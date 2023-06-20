using GameScene.PlayerControl;
using SceneManagement;
using UnityEngine;

namespace General.Grid
{
    public class MovableGridEntityManager<T> : GridEntityManager<T> where T : MovableGridEntity
    {
        public void AddAtAndMoveTo(T entity, Vector2Int index)
        {
            entity.MoveTo(index, GetPositionForIndex(index));
            _entities.Add(entity);
        }
    }
}