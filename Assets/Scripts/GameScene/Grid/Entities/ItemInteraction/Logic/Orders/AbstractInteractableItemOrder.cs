using System.Collections.Generic;
using GameScene.SpecialGridEntities.PickPoints;
using UnityEngine;

namespace GameScene.Grid.Entities.ItemInteraction.Logic
{
    public abstract class AbstractInteractableItemOrder: ScriptableObject
    {
        public abstract bool IsFeasibleOrder(List<InteractableItemType> order);
    }
}