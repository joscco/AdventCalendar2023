using System.Collections.Generic;
using System.Linq;
using GameScene.Grid.Entities.ItemInteraction.Logic;
using UnityEngine;

namespace GameScene.SpecialGridEntities.PickPoints
{
    [CreateAssetMenu(fileName = "ItemOrder", menuName = "Item/MultiOrder")]
    public class InteractableItemStackMultiOrder : AbstractInteractableItemOrder
    {
        // Order in which the items may be stacked: a > b > c > d
        public List<InteractableItemStackOrder> allowedOrders;
        
        public override bool IsFeasibleOrder(List<InteractableItemType> order)
        {
            return allowedOrders.Any(allowedOrder => allowedOrder.IsFeasibleOrder(order));
        }
    }
}