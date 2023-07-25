using System.Linq;
using GameScene.Grid.Entities.ItemInteraction;
using GameScene.Grid.Managers;
using UnityEngine;

namespace GameScene.SpecialGridEntities.EntityManagers
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
            entity.MoveTo(index, GetPositionForIndex(index));
            entities.Add(entity);
        }
    }
}