using GameScene.Grid.Managers;
using UnityEngine;

namespace GameScene.Grid.Entities.ItemInteraction
{
    public class InteractableItemManager : GridEntityManager<InteractableItem>
    {
        public void AddAtAndMoveTo(InteractableItem entity, Vector2Int index)
        {
            entity.transform.SetParent(transform);
            entity.MoveTo(index, GetBasePositionForIndex(index));
            entities.Add(entity);
        }
    }
}