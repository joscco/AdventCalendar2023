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

        private void Start()
        {
            SetupLevel();
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
            if (groundMap.HasTileAt(nextIndex) && !obstacleMap.HasTileAt(nextIndex))
            {
                var neighboringIndices = groundMap.GetNeighboringIndices(nextIndex);
                var neighboringSheep = neighboringIndices
                    .Where(index => sheepManager.HasAt(index))
                    .Select(index => sheepManager.GetAt(index));

                var potentialNextIndex = new Dictionary<MovableGridEntity, Vector2Int>();

                foreach (var sheep in neighboringSheep)
                {
                    var sheepNeighbors = groundMap.GetHVNeighboringIndices(sheep.GetIndex());
                    Debug.Log(sheepNeighbors.Count);
                    var bestNeighbor = sheepNeighbors
                        .Where(index => !potentialNextIndex.Values.Contains(index))
                        .Where(index => index != currentIndex)
                        .Where(index => index != nextIndex)
                        .Where(index => !obstacleMap.HasTileAt(index))
                        .OrderByDescending(index => (nextIndex - index).magnitude)
                        .First();

                    if (null == bestNeighbor)
                    {
                        // We cannot move this way then
                        return;
                    }
                    
                    potentialNextIndex.Add(sheep, bestNeighbor);
                }
                
                // Move

                foreach (var sheep in neighboringSheep)
                {
                    var nextSheepIndex = potentialNextIndex[sheep];
                    sheep.UpdatePosition(nextSheepIndex, grid.GetPositionForIndex(nextSheepIndex));
                }

                player.UpdatePosition(nextIndex, grid.GetPositionForIndex(nextIndex));
            }
        }
    }
}