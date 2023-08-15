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
            entity.SetIndicesAndPosition(index, GetBasePositionForIndex(index));
            entities.Add(entity);
        }

        public bool HasAt(Vector2Int index)
        {
            return entities.Any(entity => entity.GetMainIndex() == index);
        }

        public T GetAt(Vector2Int index)
        {
            return entities.First(entity => entity.GetMainIndex() == index);
        }

        public void RemoveItem(T item)
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
    }
}