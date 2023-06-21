using System.Collections.Generic;
using System.Linq;
using GameScene.PlayerControl;
using GameScene.SpecialGridEntities;
using GameScene.SpecialGridEntities.EntityManagers;
using General.Grid;
using Levels.SheepLevel;
using SceneManagement;
using UnityEngine;

namespace GameScene
{
    public class ElfLevelManager : LevelManager
    {
        // Infrastructure
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
        [SerializeField] private GridEntityPicker player;

        // Game State
        private bool _setup;
        private bool _hasWon;
        private bool _hasLost;

        // Free Indices is a subset of all feasible indices consisting of all that are not blocked
        private List<Vector2Int> _feasibleIndices;
        private List<Vector2Int> _freeIndices;

        private void Start()
        {
            SetupLevel();
            CalculateGameStatus();
        }

        private void CalculateGameStatus()
        {
            _feasibleIndices = GetFreeIndicesForMoving();
            _freeIndices = _feasibleIndices.Where(index => !collidersMap.HasTileAt(index))
                .Where(index => !pushablesManager.HasAt(index))
                .Where(index => !pickPointsManager.HasAt(index))
                .ToList();

            if (!_feasibleIndices.Contains(player.GetMainIndex()))
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

        private List<Vector2Int> GetFreeIndicesForMoving()
        {
            List<Vector2Int> result = new();
            result.AddRange(groundMap.GetIndices());
            result.AddRange(slidingGroundMap.GetIndices());
            result.AddRange(toggleableTilesManager.GetActiveTileIndices());

            collidersMap.GetIndices().ForEach(index => result.Remove(index));
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
            player.InstantUpdatePosition(index, grid.GetPositionForIndex(index));
            player.StartShaking();
        }

        private void InitToggleableTiles()
        {
        }

        private void InitToggleableTileSwitches()
        {
        }

        private void InitPushableToyParts()
        {
            var pushables = pushablesManager.GetComponentsInChildren<PushableGridEntity>();
            foreach (var pushable in pushables)
            {
                var index = grid.FindNearestIndexForPosition(pushable.transform.position);
                pushablesManager.AddAt(pushable, index);
            }
        }

        public void InitPortals()
        {
        }

        public void InitPickupPoints()
        {
            var pickpoints = pickPointsManager.GetComponentsInChildren<PickPoint>();
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

                if (move != Vector2Int.zero)
                {
                    HandlePlayerMove(move);
                    CalculateGameStatus();
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
                if (player.HasItemToGive() && pickPoint.CanTakeItem(player.GetItem()))
                {
                    var item = player.GetItem();
                    player.RemoveItem(item);
                    pickPoint.GiveItem(item);
                }
                else if (pickPoint.HasItemToGive() && player.CanTakeItem(pickPoint.GetItem()))
                {
                    var item = pickPoint.GetItem();
                    pickPoint.RemoveItem(item);
                    player.GiveItem(item);
                }

                return;
            }

            // Interacting with Toggle
            if (toggleableTileSwitchManager.HasAt(nextIndex))
            {
                var toggleSwitch = toggleableTileSwitchManager.GetAt(nextIndex);
                toggleSwitch.Toggle();
                return;
            }

            // Moving Stuff, tiles must be feasible to move there
            if (!_feasibleIndices.Contains(nextIndex))
            {
                // Player cannot move here -> Stop
                return;
            }

            // Pushing over sliding
            if (pushablesManager.HasAt(nextIndex))
            {
                var overNextIndex = nextIndex + direction;
                if (!_feasibleIndices.Contains(overNextIndex))
                {
                    // Pushable cannot be moved
                    return;
                }

                var pushableToMove = pushablesManager.GetAt(nextIndex);
                var offset = pushableToMove.GetMainIndex() - nextIndex;

                if (slidingGroundMap.HasTileAt(overNextIndex))
                {
                    var nextSlidingIndexForItem = FindNextFreeNonSlidingTileInDirection(overNextIndex, direction);
                    pushableToMove.MoveTo(
                        nextSlidingIndexForItem + offset,
                        grid.GetPositionForIndex(nextSlidingIndexForItem + offset)
                    );
                    player.MoveTo(nextIndex, grid.GetPositionForIndex(nextIndex));
                    return;
                }

                // Normal push
                var nextMainIndexForPushable = overNextIndex + offset;
                var feasibleIndicesPlusSelf = _feasibleIndices.Concat(pushableToMove.GetCoveredIndices()).ToList();
                var allIndicesAfterPushAreFree = pushableToMove.GetCoveredIndicesWhenMainIndexWas(
                    nextMainIndexForPushable).All(index => feasibleIndicesPlusSelf.Contains(index));
                if (allIndicesAfterPushAreFree)
                {
                    pushableToMove.MoveTo(
                        overNextIndex + offset,
                        grid.GetPositionForIndex(overNextIndex + offset)
                    );
                    player.MoveTo(nextIndex, grid.GetPositionForIndex(nextIndex));
                }

                return;
            }

            // Sliding without pushing
            if (slidingGroundMap.HasTileAt(nextIndex))
            {
                var nextSlidingIndex = FindNextFreeNonSlidingTileInDirection(nextIndex, direction);
                player.MoveTo(nextSlidingIndex, grid.GetPositionForIndex(nextSlidingIndex));
                return;
            }

            // Last case, simple movement
            if (_freeIndices.Contains(nextIndex))
            {
                player.MoveTo(nextIndex, grid.GetPositionForIndex(nextIndex));
            }
        }

        private Vector2Int FindNextFreeNonSlidingTileInDirection(Vector2Int nextIndex, Vector2Int direction)
        {
            var result = nextIndex;
            while (slidingGroundMap.HasTileAt(result) && _freeIndices.Contains(result + direction))
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

        public List<Vector2Int> GetHVNeighboringIndices(Vector2Int index)
        {
            List<Vector2Int> rawList = new List<Vector2Int>
            {
                new(index.x, index.y - 1),
                new(index.x - 1, index.y),
                new(index.x + 1, index.y),
                new(index.x, index.y + 1),
            };

            return rawList;
        }
    }
}