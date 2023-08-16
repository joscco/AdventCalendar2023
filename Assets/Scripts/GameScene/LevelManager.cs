using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using GameScene.Dialog;
using GameScene.Dialog.Area;
using GameScene.Facts;
using GameScene.Grid.Entities.ItemInteraction;
using GameScene.Grid.Entities.ItemInteraction.Logic.Checkes;
using GameScene.Grid.Entities.Obstacles;
using GameScene.Grid.Entities.Player;
using GameScene.Grid.Entities.Shared;
using GameScene.Grid.Managers;
using GameScene.SpecialGridEntities;
using GameScene.SpecialGridEntities.EntityManagers;
using Levels.SheepLevel;
using Levels.WizardLevel;
using UnityEngine;

namespace GameScene
{
    public class LevelManager : MonoBehaviour, ILevelManager
    {
        // Infrastructure
        [SerializeField] private DialogManager dialogManager;
        [SerializeField] private FactManager factManager;
        private readonly List<DialogArea> _dialogAreas = new();

        [SerializeField] private InputManager inputManager;

        [SerializeField] private CheckerManager checkerManager;
        [SerializeField] private List<Checker> checkers;

        [SerializeField] private ColoredTileManager coloredTileManager;

        [SerializeField] private GridAdapter grid;
        [SerializeField] private TilemapManager groundMap;
        [SerializeField] private TilemapManager slidingGroundMap;
        [SerializeField] private TemporaryObstacleManager obstacleManager;
        [SerializeField] private ToggleableTileManager toggleableTilesManager;
        [SerializeField] private ToggleableTileSwitchManager toggleableTileSwitchManager;
        [SerializeField] private PortalManager portalManager;
        [SerializeField] private InteractableItemManager interactableItemManager;

        [SerializeField] private List<FactCondition> factsToListenToForWin;
        [SerializeField] private Player player;

        // Game State
        private bool _setup;
        private bool _hasWon;

        // Free Indices is a subset of all feasible indices consisting of all that are not blocked
        private HashSet<Vector2Int> _existingFields;
        private HashSet<Vector2Int> _freeFields;


        private void Start()
        {
            SetupLevel();
            CalculateGameStatus();
        }

        private void Win()
        {
            player.PlayWinAnimation();
            _hasWon = true;
        }

        private void CalculateGameStatus()
        {
            _existingFields = GetExistingFields();

            _freeFields = _existingFields
                .Except(obstacleManager.GetCoveredIndices())
                .Except(toggleableTileSwitchManager.GetMainIndices())
                .ToHashSet();

            var itemMap = interactableItemManager.GetEntities()
                .ToDictionary(item => item.GetMainIndex(), item => item.GetItemType());

            checkerManager.GetEntities().ForEach(checker => checker.Check(itemMap));
        }

        private HashSet<Vector2Int> GetExistingFields()
        {
            return groundMap.GetIndices()
                .Concat(slidingGroundMap.GetIndices())
                .Concat(toggleableTilesManager.GetActiveTileIndices())
                .Concat(coloredTileManager.GetMainIndices())
                .ToHashSet();
        }

        public void SetupLevel()
        {
            InitDialogAreas();
            InitToggleableTiles();
            InitToggleableTileSwitches();
            InitColoredTiles();
            InitPlayer();
            InitPortals();
            InitObstacles();
            InitInteractables();
            InitCheckers();

            factManager.onNewFacts += _ =>
            {
                if (factManager.ConditionsAreMet(factsToListenToForWin)) Win();
            };

            _setup = true;
        }

        

        private void InitPlayer()
        {
            var index = grid.FindNearestIndexForPosition(player.transform.position);
            player.SetIndicesAndPosition(index, grid.GetBasePositionForIndex(index));
        }

        private void InitDialogAreas()
        {
            var foundAreas = FindObjectsOfType<DialogArea>();
            _dialogAreas.AddRange(foundAreas);

            foreach (var area in foundAreas)
            {
                var index = grid.FindNearestIndexForPosition(area.transform.position);
                area.onFactPublish += factManager.PublishFactAndUpdate;
                area.SetIndex(index);
            }
        }
        
        private void InitColoredTiles()
        {
            var coloredTiles = FindObjectsOfType<ColoredTile>();
            foreach (var tile in coloredTiles)
            {
                var index = grid.FindNearestIndexForPosition(tile.transform.position);
                coloredTileManager.AddAt(tile, index);
            }
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

        public void InitObstacles()
        {
            var obstacles = FindObjectsOfType<TemporaryObstacle>();
            foreach (var obstacle in obstacles)
            {
                var index = grid.FindNearestIndexForPosition(obstacle.transform.position);
                obstacleManager.AddAt(obstacle, index);
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
        
        private void InitCheckers()
        {
            var checkersAvailable = FindObjectsOfType<Checker>();
            foreach (var availableChecker in checkersAvailable)
            {
                var index = grid.FindNearestIndexForPosition(availableChecker.transform.position);
                checkerManager.AddAt(availableChecker, index);
            }
            
            foreach (var checker in checkers)
            {
                checker.OnFirstSuccessfulCheck += factManager.PublishFactAndUpdate;
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
            if (_setup)
            {
                var move = inputManager.GetMoveDirection();

                if (move != Vector2Int.zero && !player.IsPortaling())
                {
                    if (inputManager.IsHoldingShift())
                    {
                        SwapMovePlayer(move);
                    }
                    else
                    {
                        MovePlayer(move);
                    }
                }

                if (inputManager.JustPressedSpace())
                {
                    HandlePickUp();
                    return;
                }

                if (inputManager.JustPressedE())
                {
                    if (dialogManager.HasCurrentDialog())
                    {
                        dialogManager.ContinueDialog();
                    }
                }
            }
        }

        private void HandlePickUp()
        {
            var playerIndex = player.GetMainIndex();
            if (interactableItemManager.HasAt(playerIndex))
            {
                // Take item is the only option
                var item = interactableItemManager.GetAt(playerIndex);

                if (item.IsInteractable())
                {
                    TakeItem(item);
                }
            }
            else if (player.IsBearingItem())
            {
                var topPlayerItem = player.GetTopItem();
                DropItemAt(topPlayerItem, playerIndex);
            }
        }

        // Player can hold up to one item at most
        private void SwapMovePlayer(Vector2Int direction)
        {
            var currentIndex = player.GetMainIndex();
            var nextIndex = player.GetMainIndex() + direction;
            
            if (interactableItemManager.HasAt(nextIndex))
            {
                var item1 = interactableItemManager.GetAt(nextIndex);
                if (interactableItemManager.HasAt(currentIndex))
                {
                    var item2 = interactableItemManager.GetAt(currentIndex);
                    MoveItemTo(item2, nextIndex);
                }
                MoveItemTo(item1, currentIndex);
            }


            MovePlayer(direction);
            CalculateGameStatus();
        }

        private void MovePlayer(Vector2Int direction)
        {
            var currentIndex = player.GetMainIndex();
            var nextIndex = currentIndex + direction;

            player.SwapFaceDirectionIfNecessary(nextIndex.x);

            // Interacting with Toggle
            if (toggleableTileSwitchManager.HasAt(nextIndex))
            {
                var toggleSwitch = toggleableTileSwitchManager.GetAt(nextIndex);
                Toggle(toggleSwitch);
                return;
            }

            // Moving Stuff, tiles must be feasible to move there
            if (!_existingFields.Contains(nextIndex))
            {
                // Player cannot move here -> Stop
                return;
            }

            // Sliding
            if (slidingGroundMap.HasTileAt(nextIndex))
            {
                var nextSlidingIndex = FindNextFreeNonSlidingTileInDirection(nextIndex, direction);
                MovePlayerTo(nextSlidingIndex);
                return;
            }

            // Last case, simple movement
            if (_freeFields.Contains(nextIndex))
            {
                MovePlayerTo(nextIndex);
            }
        }

        private void InstantMoveTo(MovableGridEntity entity, Vector2Int index)
        {
            entity.InstantUpdatePosition(index, grid.GetBasePositionForIndex(index));
        }

        private void DropItemAt(InteractableItem item, Vector2Int index)
        {
            player.RemoveItem(item);
            
            item.transform.SetParent(transform);
            interactableItemManager.AddAtAndMoveTo(item, index);

            // Move has to be called AFTER dropping the item
            MovePlayerTo(index);
            
            CalculateGameStatus();
        }

        private void TakeItem(InteractableItem item)
        {
            interactableItemManager.RemoveItem(item);
            
            item.transform.parent = player.GetOffsettablePart();
            item.RelativeMoveTo(Vector2Int.zero, player.GetRelativeTop() * Vector3.up);
            
            player.TopWithItem(item);
            player.MoveTo(player.GetMainIndex(), player.transform.position, 0);
            
            CalculateGameStatus();
        }

        private void Toggle(ToggleableTileSwitch switchy)
        {
            switchy.Toggle();
            CalculateGameStatus();
        }

        private void MovePlayerTo(Vector2Int nextIndex)
        {
            var oldIndex = player.GetMainIndex();
            _dialogAreas.ForEach(area => area.OnPlayerMove(oldIndex, nextIndex));

            float verticalOffset = 0;

            if (interactableItemManager.HasAt(nextIndex))
            {
                verticalOffset = InteractableItem.ItemJumpHeight;
            }

            player.MoveTo(nextIndex, grid.GetBasePositionForIndex(nextIndex), verticalOffset);

            if (portalManager.HasAt(nextIndex))
            {
                UsePortal(player, nextIndex);
            }

            CalculateGameStatus();
        }

        private void MoveItemTo(InteractableItem item, Vector2Int index)
        {
            item.RelativeMoveTo(index, grid.GetBasePositionForIndex(index));
            if (portalManager.HasAt(index))
            {
                UsePortal(item, index);
            }
        }

        private void UsePortal(MovableGridEntity entity, Vector2Int index)
        {
            var nextIndex = portalManager.FindNextPortalIndexFor(index);

            // Only move if exit is fre
            if (_freeFields.Contains(nextIndex))
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
            while (slidingGroundMap.HasTileAt(result) && _freeFields.Contains(result + direction))
            {
                result += direction;
            }

            return result;
        }

        public bool HasWon()
        {
            return _hasWon;
        }
    }
}