using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using DG.Tweening;
using GameScene.Dialog;
using GameScene.Dialog.Area;
using GameScene.Facts;
using GameScene.Grid;
using GameScene.Grid.Entities.Essentials;
using GameScene.Grid.Entities.ItemInteraction;
using GameScene.Grid.Entities.ItemInteraction.Logic.Checkes;
using GameScene.Grid.Entities.Obstacles;
using GameScene.Grid.Entities.Shared;
using GameScene.Grid.Entities.ToggleableTile;
using GameScene.Grid.Managers;
using GameScene.SpecialGridEntities.EntityManagers;
using Levels.WizardLevel;
using UnityEngine;

namespace GameScene
{
    public class LevelManager : MonoBehaviour
    {
        // Infrastructure
        [SerializeField] private DialogManager dialogManager;
        [SerializeField] private CinemachineVirtualCamera playerCam;
        [SerializeField] private FactManager factManager;

        [SerializeField] private InputManager inputManager;

        [SerializeField] private ToggleableTileManager toggleableTilesManager;
        [SerializeField] private ToggleableTileSwitchManager toggleableTileSwitchManager;
        [SerializeField] private WallManager wallManager;
        [SerializeField] private PortalManager portalManager;
        [SerializeField] private InteractableItemManager interactableItemManager;

        // Prefabs
        [SerializeField] private GrassFloor grassFloorPrefab;
        [SerializeField] private Wall wallPrefab;
        [SerializeField] private PuzzleArea puzzleAreaPrefab;
        [SerializeField] private MultiTagChecker tagCheckerPrefab;
        [SerializeField] private ColoredTile tilePrefab;

        [SerializeField] private Player player;
        [SerializeField] private Star winStar;

        // Game State
        private bool _setup;
        private bool _hasWon;

        // To be resolved at start
        private GridAdapter _grid;
        private List<PuzzleArea> _puzzleAreas = new List<PuzzleArea>();
        private readonly List<DialogArea> _dialogAreas = new List<DialogArea>();


        private void Awake()
        {
            SetupLevel();
        }

        private void Start()
        {
            CalculateGameStatus();
        }

        private void CalculateGameStatus()
        {
            var itemMap = interactableItemManager.GetEntities()
                .ToDictionary(item => item.GetMainIndex(), item => item);

            _puzzleAreas.ForEach(area => area.Check(itemMap, player.GetMainIndex()));

            if (player.GetMainIndex() == winStar.GetMainIndex())
            {
                Win();
            }
        }

        private void Win()
        {
            winStar.transform.DOScale(1.2f, 0.5f)
                .SetEase(Ease.InOutBack)
                .SetLoops(-1, LoopType.Yoyo);
            _hasWon = true;
        }

        private bool FieldIsFreeAt(Vector2Int index)
        {
            return FieldExistsAt(index)
                   && !wallManager.HasBlockedAt(index)
                   && !toggleableTilesManager.HasAt(index);
        }

        private bool FieldExistsAt(Vector2Int vector2Int)
        {
            return _grid.groundMap.HasTileAt(vector2Int)
                   || _grid.slideMap.HasTileAt(vector2Int)
                   || toggleableTilesManager.HasActiveAt(vector2Int);
        }

        public void SetupLevel()
        {
            var gridAdapter = FindObjectOfType<GridAdapter>();

            if (!gridAdapter)
            {
                Debug.LogError("No grid adaptor was found!");
            }

            // Grid is the base of all items. Set it first!
            _grid = gridAdapter;

            InitToggleableTiles();
            InitToggleableTileSwitches();
            InitWalls();
            InitPortals();
            InitInteractables();

            // Puzzle Areas after colored tiles and walls!
            InitPuzzleAreas();
            InitPlayer();
            InitWinStar();

            InitDialogAreas();

            _setup = true;
        }

        private void InitWalls()
        {
            foreach (var wallIndex in _grid.wallMap.GetIndices())
            {
                var wall = Instantiate(wallPrefab, transform);
                wallManager.AddAt(wall, wallIndex, _grid.GetBasePositionForIndex(wallIndex));
            }

            _grid.wallMap.Hide();
        }

        private void InitPuzzleAreas()
        {
            var areaParents = FindObjectsOfType<AreaParent>().ToList();

            foreach (var areaParent in areaParents)
            {
                var newPuzzleArea = Instantiate(puzzleAreaPrefab, transform);

                var areaRects = areaParent.GetComponentsInChildren<AreaRect>().ToList();

                var indicesForThisArea = areaRects.Select(area => area.GetBounds())
                    .SelectMany(GetIndicesFromBounds)
                    .ToHashSet();

                var grassFloorList = new List<GrassFloor>();
                foreach (var index in indicesForThisArea)
                {
                    var newGrass = Instantiate(grassFloorPrefab, transform);
                    newGrass.SetIndicesAndPosition(index, _grid.GetBasePositionForIndex(index));
                    grassFloorList.Add(newGrass);
                }

                newPuzzleArea.SetGrassFloorsInChargeOf(grassFloorList);

                var wallList = wallManager.GetEntities()
                    .Where(wall => indicesForThisArea.Contains(wall.GetMainIndex()))
                    .ToList();
                newPuzzleArea.SetWallsInChargeOf(wallList);

                // Set up coloredTiles and checkers
                var checkerAreas = areaParent.GetComponentsInChildren<CheckerArea>();
                var indexList = new HashSet<Vector2Int>();
                var checkersPerIndex = new Dictionary<Vector2Int, HashSet<CheckerArea>>();

                foreach (var checkerArea in checkerAreas)
                {
                    var areaIndices = GetIndicesFromBounds(checkerArea.GetBounds());

                    foreach (var index in areaIndices)
                    {
                        indexList.Add(index);
                        if (checkersPerIndex.ContainsKey(index))
                        {
                            checkersPerIndex[index].Add(checkerArea);
                        }
                        else
                        {
                            checkersPerIndex.Add(index, new HashSet<CheckerArea> { checkerArea });
                        }
                    }
                }

                var checkerList = new List<MultiTagChecker>();

                foreach (var index in indexList)
                {
                    var checker = Instantiate(tagCheckerPrefab, newPuzzleArea.transform);
                    checker.SetIndicesAndPosition(index, _grid.GetBasePositionForIndex(index));
                    checker.SetDemandedTags(checkersPerIndex[index]
                        .Where(checker => null != checker.demandedTag)
                        .Select(checker => checker.demandedTag)
                        .ToList());
                    checkerList.Add(checker);

                    var colors = checkersPerIndex[index].ToList();
                    for (int i = 0; i < colors.Count; i++)
                    {
                        var areaChecker = colors[i];
                        var colorTile = Instantiate(tilePrefab, areaChecker.transform);
                        colorTile.SetScheme(areaChecker.color, areaChecker.pattern);
                        colorTile.transform.position = _grid.GetBasePositionForIndex(index);
                    }
                }

                newPuzzleArea.SetCheckers(checkerList);

                _puzzleAreas.Add(newPuzzleArea);
            }
        }

        private IEnumerable<Vector2Int> GetIndicesFromBounds(Bounds bounds)
        {
            var bottomLeft = _grid.FindNearestIndexForPosition(bounds.min);
            var topRight = _grid.FindNearestIndexForPosition(bounds.max);
            var list = new List<Vector2Int>();
            for (var x = bottomLeft.x; x <= topRight.x; x++)
            {
                for (var y = bottomLeft.y; y <= topRight.y; y++)
                {
                    list.Add(new Vector2Int(x, y));
                }
            }

            return list;
        }

        private void InitWinStar()
        {
            var starPos = FindObjectOfType<StarPlaceholder>();
            var index = _grid.FindNearestIndexForPosition(starPos.transform.position);
            winStar.SetIndicesAndPosition(index, _grid.GetBasePositionForIndex(index));
            starPos.transform.localScale = Vector3.zero;
        }

        private void InitPlayer()
        {
            var playerPos = _grid.GetComponentInChildren<PlayerPlaceholder>();
            var index = _grid.FindNearestIndexForPosition(playerPos.transform.position);
            player.SetIndicesAndPosition(index, _grid.GetBasePositionForIndex(index));
            playerPos.transform.localScale = Vector3.zero;
        }

        private void InitDialogAreas()
        {
            var foundAreas = FindObjectsOfType<DialogArea>();
            _dialogAreas.AddRange(foundAreas);

            foreach (var area in foundAreas)
            {
                var index = _grid.FindNearestIndexForPosition(area.transform.position);
                area.onFactPublish += factManager.PublishFactAndUpdate;
                area.SetIndex(index);
                area.OnPlayerMove(Vector2Int.zero, player.GetMainIndex());
            }
        }


        private void InitToggleableTiles()
        {
            AddAllFindable(toggleableTilesManager);
        }

        private void InitToggleableTileSwitches()
        {
            AddAllFindable(toggleableTileSwitchManager);
        }

        public void InitInteractables()
        {
            AddAllFindable(interactableItemManager);
        }

        public void InitPortals()
        {
            AddAllFindable(portalManager);
        }

        private void AddAllFindable<T>(GridEntityManager<T> manager) where T : GridEntity
        {
            var items = FindObjectsOfType<T>();
            foreach (var item in items)
            {
                var index = _grid.FindNearestIndexForPosition(item.transform.position);
                var newPos = _grid.GetBasePositionForIndex(index);
                manager.AddAt(item, index, newPos);
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
            if (!FieldExistsAt(nextIndex))
            {
                // Player cannot move here -> Stop
                return;
            }

            // Sliding
            if (_grid.slideMap.HasTileAt(nextIndex))
            {
                var nextSlidingIndex = FindNextFreeNonSlidingTileInDirection(nextIndex, direction);
                MovePlayerTo(nextSlidingIndex);
                return;
            }

            // Last case, simple movement
            if (FieldIsFreeAt(nextIndex))
            {
                MovePlayerTo(nextIndex);
            }
        }

        private void InstantMoveTo(MovableGridEntity entity, Vector2Int index)
        {
            entity.InstantUpdatePosition(index, _grid.GetBasePositionForIndex(index));
        }

        private void DropItemAt(InteractableItem item, Vector2Int index)
        {
            player.RemoveItem(item);

            item.transform.SetParent(transform);
            interactableItemManager.AddAtAndMoveTo(item, index, _grid.GetBasePositionForIndex(index));

            // Move has to be called AFTER dropping the item
            MovePlayerTo(index);

            CalculateGameStatus();
        }

        private void TakeItem(InteractableItem item)
        {
            item.Uncheck();
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

            player.MoveTo(nextIndex, _grid.GetBasePositionForIndex(nextIndex), verticalOffset);

            if (portalManager.HasAt(nextIndex))
            {
                UsePortal(player, nextIndex);
            }

            CalculateGameStatus();
        }

        private void MoveItemTo(InteractableItem item, Vector2Int index)
        {
            item.RelativeMoveTo(index, _grid.GetBasePositionForIndex(index));
            if (portalManager.HasAt(index))
            {
                UsePortal(item, index);
            }
        }

        private void UsePortal(MovableGridEntity entity, Vector2Int index)
        {
            var nextIndex = portalManager.FindNextPortalIndexFor(index);

            // Only move if exit is fre
            if (FieldIsFreeAt(nextIndex))
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
            while (_grid.slideMap.HasTileAt(result) && FieldIsFreeAt(result + direction))
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