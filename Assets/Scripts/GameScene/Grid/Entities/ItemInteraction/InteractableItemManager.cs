using GameScene.Grid.Managers;
using UnityEngine;

namespace GameScene.Grid.Entities.ItemInteraction
{
    public class InteractableItemManager : GridEntityManager<InteractableItem>
    {
        public void AddAtAndMoveTo(InteractableItem entity, Vector2Int index)
        {
            entity.RelativeMoveTo(index, GetBasePositionForIndex(index));
            entities.Add(entity);
        }
    }
}