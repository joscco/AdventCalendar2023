using System.Collections.Generic;
using System.Linq;
using GameScene.Grid.Entities.Shared;
using SceneManagement;
using UnityEngine;

namespace GameScene.Grid.Managers
{
    public class GridEntityManager<T> : GridAdapter where T : GridEntity
    {
        protected readonly List<T> entities = new();

        public void AddAt(T entity, Vector2Int index)
        {
            entity.SetIndicesAndPosition(index, GetPositionForIndex(index));
            entities.Add(entity);
        }

        public bool HasAt(Vector2Int index)
        {
            return entities.Any(entity => entity.GetCoveredIndices().Contains(index));
        }

        public T GetAt(Vector2Int index)
        {
            return entities.First(entity => entity.GetCoveredIndices().Contains(index));
        }

        public void Release(T item)
        {
            entities.Remove(item);
        }

        public List<T> GetEntities()
        {
            return entities;
        }
        
        public List<Vector2Int> GetMainIndices()
        {
            return entities.Select(entity => entity.GetMainIndex()).ToList();
        }
        
        public virtual HashSet<Vector2Int> GetCoveredIndices()
        {
            return entities.SelectMany(entity => entity.GetCoveredIndices()).ToHashSet();
        }
    }
}