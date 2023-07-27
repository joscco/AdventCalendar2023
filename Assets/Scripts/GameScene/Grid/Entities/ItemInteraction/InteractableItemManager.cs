using System.Linq;
using GameScene.Grid.Managers;
using UnityEngine;

namespace GameScene.Grid.Entities.ItemInteraction
{
    public class InteractableItemManager : GridEntityManager<InteractableItem>
    {
        public bool HasPushableAt(Vector2Int index)
        {
            return entities.Any(entity => entity.GetCoveredIndices().Contains(index) && entity.IsPushable());
        }
        
        public void AddAtAndMoveTo(InteractableItem entity, Vector2Int index)
        {
            entity.transform.SetParent(transform);
            entity.MoveTo(index, GetPositionForIndex(index), true);
            entities.Add(entity);
        }
    }
}