using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using GameScene.Dialog.Background;
using GameScene.Grid.Entities;
using GameScene.Grid.Entities.Shared;
using GameScene.PlayerControl;
using GameScene.SpecialGridEntities;
using GameScene.SpecialGridEntities.EntityManagers;
using General.Grid;
using Levels.SheepLevel;
using Levels.WizardLevel;
using SceneManagement;
using UnityEngine;

namespace GameScene
{
    public class ElfLevelManager : LevelManager
    {
        // Infrastructure
        [SerializeField] private DialogManager dialogManager;
        
        [SerializeField] private GridAdapter grid;
        [SerializeField] private TilemapManager groundMap;
        [SerializeField] private TilemapManager slidingGroundMap;
        [SerializeField] private TilemapManager collidersMap;
        [SerializeField] private ToggleableTileManager toggleableTilesManager;
        [SerializeField] private ToggleableTileSwitchManager toggleableTileSwitchManager;
        [SerializeField] private PortalManager portalManager;
        [SerializeField] private PushablesManager pushablesManager;
        [SerializeField] private PickPointManager pickPointsManager;

        [SerializeField] private List<PickPoint> pickPointsToListenTo;
        [SerializeField] private Player player;

        // Game State
        private bool _setup;
        private bool _hasWon;
        private bool _hasLost;

        // Free Indices is a subset of all feasible indices consisting of all that are not blocked
        private List<Vector2Int> _potentialFreeFields;
        private List<Vector2Int> _currentFreeFields;

        private void Start()
        {
            SetupLevel();
            CalculateGameStatus();
        }

        private void CalculateGameStatus()
        {
            _potentialFreeFields = GetFieldsThePlayerCouldMoveToSometime();
            _currentFreeFields = _potentialFreeFields.Where(index => !pushablesManager.HasAt(index))
                .Where(index => !pickPointsManager.HasAt(index))
                .ToList();

            if (!_potentialFreeFields.Contains(player.GetMainIndex()))
            {
                // Player is on unfeasible position! 
                player.PlayDeathAnimation();
                _hasLost = true;
            }

            if (pickPointsToListenTo.Count != 0 && pickPointsToListenTo.All(pickPoint => pickPoint.IsComplete()))
            {
                player.PlayWinAnimation();
                _hasWon = true;
            }
        }

        private List<Vector2Int> GetFieldsThePlayerCouldMoveToSometime()
        {
            List<Vector2Int> result = new();
            result.AddRange(groundMap.GetIndices());
            result.AddRange(slidingGroundMap.GetIndices());
            result.AddRange(toggleableTilesManager.GetActiveTileIndices());

            collidersMap.GetIndices().ForEach(index => result.Remove(index));
            toggleableTileSwitchManager.GetMainIndices().ForEach(index => result.Remove(index));
            return result;
        }

        public void SetupLevel()
        {
            InitToggleableTiles();
            InitToggleableTileSwitches();
            InitPlayer();
            InitPortals();
            InitPickupPoints();
            InitPushableToyParts();

            _setup = true;
        }

        private void InitPlayer()
        {
            var index = grid.FindNearestIndexForPosition(player.transform.position);
            player.SetIndicesAndPosition(index, grid.GetPositionForIndex(index));
        }

        private void InitToggleableTiles()
        {
            var toggleTile = FindObjectsOfType<ToggleableTile>();
            foreach (var tile in toggleTile)
            {
                var index = grid.FindNearestIndexForPosition(tile.transform.position);
                toggleableTilesManager.AddAt(tile, index);
            }
        }

        private void InitToggleableTileSwitches()
        {
            var switches = FindObjectsOfType<ToggleableTileSwitch>();
            foreach (var switchy in switches)
            {
                var index = grid.FindNearestIndexForPosition(switchy.transform.position);
                toggleableTileSwitchManager.AddAt(switchy, index);
            }
        }

        private void InitPushableToyParts()
        {
            var pushables = FindObjectsOfType<PushableGridEntity>();
            foreach (var pushable in pushables)
            {
                var index = grid.FindNearestIndexForPosition(pushable.transform.position);
                pushablesManager.AddAt(pushable, index);
            }
        }

        public void InitPortals()
        {
            var portals = FindObjectsOfType<Portal>();
            foreach (var portal in portals)
            {
                var index = grid.FindNearestIndexForPosition(portal.transform.position);
                portalManager.AddAt(portal, index);
            }
        }

        public void InitPickupPoints()
        {
            var pickpoints = FindObjectsOfType<PickPoint>();
            foreach (var pickpoint in pickpoints)
            {
                var index = grid.FindNearestIndexForPosition(pickpoint.transform.position);
                pickPointsManager.AddAt(pickpoint, index);
            }
        }

        public override void HandleUpdate()
        {
            if (_setup && null != InputManager.instance)
            {
                var move = InputManager.instance.GetMoveDirection();

                if (move != Vector2Int.zero && !player.IsPortaling())
                {
                    HandlePlayerMove(move);
                }

                if (InputManager.instance.GetE() && dialogManager.HasCurrentDialog())
                {
                    dialogManager.ContinueDialog();
                }
            }
        }

        private void HandlePlayerMove(Vector2Int direction)
        {
            var currentIndex = player.GetMainIndex();
            var nextIndex = currentIndex + direction;

            // Interacting with PickPoint
            if (pickPointsManager.HasAt(nextIndex))
            {
                var pickPoint = pickPointsManager.GetAt(nextIndex);
                ExchangeItem(player, pickPoint);
                return;
            }

            // Interacting with Toggle
            if (toggleableTileSwitchManager.HasAt(nextIndex))
            {
                var toggleSwitch = toggleableTileSwitchManager.GetAt(nextIndex);
                Toggle(toggleSwitch);
                return;
            }

            // Moving Stuff, tiles must be feasible to move there
            if (!_potentialFreeFields.Contains(nextIndex))
            {
                // Player cannot move here -> Stop
                return;
            }

            // Pushing over sliding
            if (pushablesManager.HasAt(nextIndex))
            {
                var overNextIndex = nextIndex + direction;
                if (!_potentialFreeFields.Contains(overNextIndex) || player.HasItemToGive())
                {
                    // Pushable cannot be moved
                    return;
                }

                var pushableToMove = pushablesManager.GetAt(nextIndex);
                var offset = pushableToMove.GetMainIndex() - nextIndex;

                if (slidingGroundMap.HasTileAt(overNextIndex))
                {
                    var nextSlidingIndexForItem = FindNextFreeNonSlidingTileInDirection(overNextIndex, direction);
                    MoveTo(pushableToMove, nextSlidingIndexForItem + offset);
                    MoveTo(player, nextIndex);
                    return;
                }

                // Normal push
                var nextMainIndexForPushable = overNextIndex + offset;
                var nextCoveredIndicesAfterPush =
                    pushableToMove.GetCoveredIndicesIfMainIndexWas(nextMainIndexForPushable);
                var currentFreeFieldsOrOwnFields = _currentFreeFields.Concat(pushableToMove.GetCoveredIndices()).ToList();
                var allIndicesAfterPushAreFree = nextCoveredIndicesAfterPush.All(currentFreeFieldsOrOwnFields.Contains);

                if (allIndicesAfterPushAreFree)
                {
                    MoveTo(pushableToMove, overNextIndex + offset);
                    MoveTo(player, nextIndex);
                }

                return;
            }

            // Sliding without pushing
            if (slidingGroundMap.HasTileAt(nextIndex))
            {
                var nextSlidingIndex = FindNextFreeNonSlidingTileInDirection(nextIndex, direction);
                MoveTo(player, nextSlidingIndex);
                return;
            }

            // Last case, simple movement
            if (_currentFreeFields.Contains(nextIndex))
            {
                MoveTo(player, nextIndex);
            }
        }


        private void InstantMoveTo(MovableGridEntity entity, Vector2Int index)
        {
            entity.InstantUpdatePosition(index, grid.GetPositionForIndex(index));
        }

        private void ExchangeItem(ItemBearer bearerA, ItemBearer bearerB)
        {
            if (bearerA.HasItemToGive() && bearerB.CanTakeItem(bearerA.GetItem()))
            {
                var item = bearerA.GetItem();
                bearerA.RemoveItem(item);
                bearerB.GiveItem(item);
            }
            else if (bearerB.HasItemToGive() && bearerA.CanTakeItem(bearerB.GetItem()))
            {
                var item = bearerB.GetItem();
                bearerB.RemoveItem(item);
                bearerA.GiveItem(item);
            }
            CalculateGameStatus();
        }

        private void Toggle(ToggleableTileSwitch switchy)
        {
            switchy.Toggle();
            CalculateGameStatus();
        }

        private void MoveTo(MovableGridEntity entity, Vector2Int index)
        {
            entity.MoveTo(index, grid.GetPositionForIndex(index));
            if (entity.IsSingleIndex() && portalManager.HasAt(index))
            {
                UsePortal(entity, index);
            }
            
            CalculateGameStatus();
        }

        private void UsePortal(MovableGridEntity entity, Vector2Int index)
        {
            var nextIndex = portalManager.FindNextPortalIndexFor(index);

            // Only move if exit is fre
            if (_currentFreeFields.Contains(nextIndex))
            {
                entity.SetPortaling(true);

                var sequence = DOTween.Sequence();
                sequence.Insert(0, entity.BlendOut())
                    .InsertCallback(0.4f, () => InstantMoveTo(entity, nextIndex))
                    .Insert(0.6f, entity.BlendIn())
                    .InsertCallback(1f, () => entity.SetPortaling(false));
            }
        }

        private Vector2Int FindNextFreeNonSlidingTileInDirection(Vector2Int nextIndex, Vector2Int direction)
        {
            var result = nextIndex;
            while (slidingGroundMap.HasTileAt(result) && _currentFreeFields.Contains(result + direction))
            {
                result += direction;
            }

            return result;
        }

        public override bool HasWon()
        {
            return _hasWon;
        }

        public override bool HasLost()
        {
            return _hasLost;
        }
    }
}