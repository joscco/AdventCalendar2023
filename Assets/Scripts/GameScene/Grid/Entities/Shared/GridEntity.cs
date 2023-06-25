using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace GameScene.Grid.Entities.Shared
{
    public class GridEntity : MonoBehaviour
    {
        [SerializeField] protected EntityRenderer entityRenderer;
        [SerializeField] protected Vector2Int currentMainIndex;
        [SerializeField] private List<Vector2Int> relativeIndicesIncluded = new() { Vector2Int.zero };

        public void SetIndicesAndPosition(Vector2Int newMainIndex, Vector2 newPosition)
        {
            currentMainIndex = newMainIndex;
            transform.position = newPosition;
            UpdateSortingOrder(-newMainIndex.y);
        }
        
        public Vector2Int GetMainIndex()
        {
            return currentMainIndex;
        }

        public List<Vector2Int> GetCoveredIndices()
        {
            return relativeIndicesIncluded.Select(index => currentMainIndex + index).ToList();
        }

        public List<Vector2Int> GetCoveredIndicesWhenMainIndexWas(Vector2Int potentialMainIndex)
        {
            return relativeIndicesIncluded.Select(index => potentialMainIndex + index).ToList();
        }

        public Tween BlendOut()
        {
            return entityRenderer.BlendOut();
        }
        
        public Tween BlendIn()
        {
            return entityRenderer.BlendIn();
        }

        protected virtual void UpdateSortingOrder(int newOrder)
        {
            entityRenderer.SetSortingOrder(newOrder);
        }
    }
}