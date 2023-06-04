using System.Collections.Generic;
using System.Linq;
using GameScene;
using GameScene.PlayerControl;
using SceneManagement;
using UnityEngine;

namespace Levels.SheepLevel
{
    public class SheepLevelManager : LevelManager
    {
        [SerializeField] private TilemapManager groundMap;
        [SerializeField] private TilemapManager obstacleMap;
        [SerializeField] private TilemapManager aimMap;

        [SerializeField] private GridAdapter grid;
        [SerializeField] private SheepManager sheepManager;
        [SerializeField] private MovableGridEntity player;
        [SerializeField] private MovableGridEntity sheepPrefab;

        [SerializeField] private Vector2Int playerStartPosition;
        [SerializeField] private List<Vector2Int> sheepPositions;

        private bool _setup;
        private SheepMoveCalculator _sheepMoveCalculator;

        private void Start()
        {
            SetupLevel();
            _sheepMoveCalculator = new SheepMoveCalculator();
        }

        public void SetupLevel()
        {
            InitPlayer();
            InitSheep();

            _setup = true;
        }

        private void InitSheep()
        {
            foreach (var sheepIndex in sheepPositions)
            {
                var sheep = Instantiate(sheepPrefab, sheepManager.transform);
                Debug.Log(sheepIndex);
                sheepManager.AddAt(sheep, sheepIndex);
            }
        }

        private void InitPlayer()
        {
            player.InstantUpdatePosition(playerStartPosition, grid.GetPositionForIndex(playerStartPosition));
        }

        public override void HandleUpdate()
        {
            if (_setup)
            {
                var move = InputManager.instance.GetMoveDirection();
                if (move != Vector2Int.zero)
                {
                    HandlePlayerMove(move);
                }
            }
        }

        private void HandlePlayerMove(Vector2Int move)
        {
            var currentIndex = player.GetIndex();
            var nextIndex = currentIndex + move;

            var possibleIndices = groundMap.GetIndices()
                .Where(index => !obstacleMap.HasTileAt(index))
                .ToList();

            if (!possibleIndices.Contains(nextIndex))
            {
                // Moving is not possible
                return;
            }

            var moveResult = _sheepMoveCalculator.CalculateNewPositions(
                currentIndex,
                nextIndex,
                possibleIndices,
                sheepManager.GetEntities()
            );

            if (moveResult.movingPossible)
            {
                // Move all sheep
                foreach (var sheepPair in moveResult.newSheepPositions)
                {
                    var sheep = sheepPair.Key;
                    var newIndex = sheepPair.Value;
                    sheep.UpdatePosition(newIndex, grid.GetPositionForIndex(newIndex));
                }

                player.UpdatePosition(nextIndex, grid.GetPositionForIndex(nextIndex));
            }
        }
    }
}