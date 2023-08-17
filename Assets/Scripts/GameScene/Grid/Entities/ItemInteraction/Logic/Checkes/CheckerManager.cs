using GameScene.Grid.Managers;
using UnityEngine;

namespace GameScene.Grid.Entities.ItemInteraction.Logic.Checkes
{
    public class CheckerManager : GridEntityManager<Checker>
    {
        public void AddAt(Checker entity, Vector2Int index)
        {
            entity.transform.SetParent(transform);
            entity.SetIndicesAndPosition(index, GetBasePositionForIndex(index));
            entities.Add(entity);
        }
    }
}