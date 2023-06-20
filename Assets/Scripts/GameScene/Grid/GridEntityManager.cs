using System.Collections.Generic;
using System.Linq;
using GameScene.PlayerControl;
using General.Grid;
using UnityEngine;

namespace SceneManagement
{
    public class GridEntityManager<T> : GridAdapter where T : GridEntity
    {
        protected List<T> _entities = new();

        public void AddAt(T entity, Vector2Int index)
        {
            entity.SetIndicesAndPosition(index, GetPositionForIndex(index));
            _entities.Add(entity);
        }

        public bool HasAt(Vector2Int index)
        {
            return _entities.Any(entity => entity.GetCoveredIndices().Contains(index));
        }

        public T GetAt(Vector2Int index)
        {
            return _entities.First(entity => entity.GetCoveredIndices().Contains(index));
        }

        public List<T> GetEntities()
        {
            return _entities;
        }
    }
}