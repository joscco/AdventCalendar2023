using System.Collections.Generic;
using System.Linq;
using GameScene.Facts;
using GameScene.Grid.Entities.Shared;
using UnityEngine;

namespace GameScene.Grid.Entities.Obstacles
{
    public abstract class TemporaryObstacle : GridEntity
    {
        [SerializeField] private bool blocking = true;
        [SerializeField] private List<Vector2Int> relativeIndicesIncluded = new() { Vector2Int.zero };
        [SerializeField] private List<FactCondition> factsToListenToForCompletion;

        public void Unblock()
        {
            blocking = false;
            OnUnblock();
        }

        public bool IsBlocking()
        {
            return blocking;
        }

        protected abstract void OnUnblock();

        public List<Vector2Int> GetCoveredIndices()
        {
            return relativeIndicesIncluded
                .Select(index => currentMainIndex + index)
                .ToList();
        }

        public List<Vector2Int> GetCoveredIndicesIfMainIndexWas(Vector2Int potentialMainIndex)
        {
            return relativeIndicesIncluded
                .Select(index => potentialMainIndex + index)
                .ToList();
        }

        public List<FactCondition> GetConditionsForCompletion()
        {
            return factsToListenToForCompletion;
        }
    }
}