using System.Collections.Generic;
using GameScene.SpecialGridEntities.PickPoints;
using UnityEngine;

namespace GameScene.Grid.Entities.ItemInteraction.Logic.Orders
{
    [CreateAssetMenu(fileName = "ItemOrder", menuName = "Item/ExactTypeOrder")]
    public class InteractableItemExactTypeOrder : AbstractInteractableItemOrder
    {
        // Order in which the items may be stacked: a > b > c > d
        public List<InteractableItemType> orderFromBottomToTop;

        public override bool IsFeasibleOrder(List<InteractableItemType> order)
        {
            if (order.Count != orderFromBottomToTop.Count)
            {
                return false;
            }

            for (int i = 0; i < orderFromBottomToTop.Count; i++)
            {
                var wantedType = orderFromBottomToTop[i];
                var givenType = order[i];
                if (wantedType != givenType)
                {
                    return false;
                }
            }

            return true;
        }
    }
}