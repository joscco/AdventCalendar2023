using System.Collections.Generic;
using System.Linq;
using GameScene.PlayerControl;
using UnityEngine;

namespace SceneManagement
{
    public class MovableGridEntityManager<T> : MonoBehaviour where T : MovableGridEntity
    {
        [SerializeField] private Grid grid;
        protected List<T> _entities = new();

        public void AddAt(T entity, Vector2Int index)
        {
            entity.InstantUpdatePosition(index, IndexToPosition(index));
            _entities.Add(entity);
        }

        public bool HasAt(Vector2Int index)
        {
            return _entities.Any(entity => entity.GetIndex() == index);
        }

        public Vector2 IndexToPosition(Vector2Int index)
        {
            return grid.GetCellCenterWorld(IndexWrap(index));
        }

        public Vector3Int IndexWrap(Vector2Int index)
        {
            return new Vector3Int(index.x, index.y, 0);
        }

        public T GetAt(Vector2Int index)
        {
            return _entities.First(entity => entity.GetIndex() == index);
        }

        public List<T> GetEntities()
        {
            return _entities;
        }
    }
}