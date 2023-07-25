using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using GameScene.Dialog;
using GameScene.Grid.Entities.ItemInteraction;
using GameScene.PlayerControl;
using GameScene.SpecialGridEntities;
using GameScene.SpecialGridEntities.EntityManagers;
using Levels.SheepLevel;
using Levels.WizardLevel;
using SceneManagement;
using UnityEngine;

namespace GameScene
{
    public class LevelManager : MonoBehaviour, ILevelManager
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
        [SerializeField] private InteractableItemManager interactableItemManager;

        [SerializeField] private List<InteractableItem> pickPointsToListenTo;
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
            _currentFreeFields = _potentialFreeFields.Except(interactableItemManager.GetCoveredIndices())
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
            InitInteractables();

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

        public void InitInteractables()
        {
            var items = FindObjectsOfType<InteractableItem>();
            foreach (var item in items)
            {
                var index = grid.FindNearestIndexForPosition(item.transform.position);
                interactableItemManager.AddAt(item, index);
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

        public void HandleUpdate()
        {
            if (_setup && null != InputManager.instance)
            {
                var move = InputManager.instance.GetMoveDirection();
                
                if (move != Vector2Int.zero && !player.IsPortaling())
                {
                    var holdingSpace = InputManager.instance.GetHoldingSpace();
                    Debug.Log("Holding Space: " + holdingSpace);
                    if (holdingSpace)
                    {
                        HandlePlayerItemInteraction(move);
                    }
                    else
                    {
                        HandlePlayerMove(move);
                    }
                }

                if (InputManager.instance.GetE() && dialogManager.HasCurrentDialog())
                {
                    dialogManager.ContinueDialog();
                }
            }
        }

        // Player can hold up to one item at most
        private void HandlePlayerItemInteraction(Vector2Int direction)
        {
            var nextIndex = player.GetMainIndex() + direction;
            Debug.Log("Handling interaction!");
            if (!player.IsBearingItem() && interactableItemManager.HasAt(nextIndex))
            {
                // Take item
                Debug.Log("Taking item!");
                var item = interactableItemManager.GetAt(nextIndex);

                if (item.IsBearingItem() && item.GetItem().IsPickable())
                {
                    ExchangeItem(player, item);
                } else if (!item.IsBearingItem() && item.IsPickable())
                {
                    TakeItem(player, item);
                }
                
            }
            else if (player.IsBearingItem())
            {

                if (_currentFreeFields.Contains(nextIndex))
                {
                    LetGoItemAt(player, nextIndex);
                }
                else if (interactableItemManager.HasAt(nextIndex))
                {
                    var item = interactableItemManager.GetAt(nextIndex);
                    var fieldCanBeToppedWithItem = item.CanBeToppedWithItem(player.GetItem());
                    if (fieldCanBeToppedWithItem)
                    {
                        ExchangeItem(player, item);
                    }
                }
            }
        }

        private void HandlePlayerMove(Vector2Int direction)
        {
            var currentIndex = player.GetMainIndex();
            var nextIndex = currentIndex + direction;

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
            if (interactableItemManager.HasPushableAt(nextIndex))
            {
                var overNextIndex = nextIndex + direction;
                if (!_potentialFreeFields.Contains(overNextIndex) || player.IsBearingItem())
                {
                    // Pushable cannot be moved
                    return;
                }

                var pushableToMove = interactableItemManager.GetAt(nextIndex);
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
                var currentFreeFieldsOrOwnFields =
                    _currentFreeFields.Concat(pushableToMove.GetCoveredIndices()).ToList();
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

        private void ExchangeItem(IInteractableItemBearer bearerA, IInteractableItemBearer bearerB)
        {
            if (bearerA.IsBearingItem() && bearerB.CanBeToppedWithItem(bearerA.GetItem()))
            {
                var item = bearerA.GetItem();
                bearerA.RemoveItem(item);
                bearerB.TopWithItem(item);
            }
            else if (bearerB.IsBearingItem() && bearerA.CanBeToppedWithItem(bearerB.GetItem()))
            {
                var item = bearerB.GetItem();
                bearerB.RemoveItem(item);
                bearerA.TopWithItem(item);
            }

            CalculateGameStatus();
        }

        private void LetGoItemAt(IInteractableItemBearer bearerA, Vector2Int index)
        {
            var item = bearerA.GetItem();
            bearerA.RemoveItem(item);
            interactableItemManager.AddAtAndMoveTo(item, index);
            CalculateGameStatus();
        }
        
        private void TakeItem(IInteractableItemBearer bearerA, InteractableItem item)
        {
            interactableItemManager.Release(item);
            bearerA.TopWithItem(item);
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

        public bool HasWon()
        {
            return _hasWon;
        }

        public bool HasLost()
        {
            return _hasLost;
        }
    }
}