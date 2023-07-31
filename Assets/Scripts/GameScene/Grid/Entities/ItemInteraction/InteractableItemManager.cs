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
        
        public void AddAtAndJumpTo(InteractableItem entity, Vector2Int index)
        {
            entity.transform.SetParent(transform);
            entity.JumpTo(index, GetPositionForIndex(index));
            entities.Add(entity);
        }
    }
}