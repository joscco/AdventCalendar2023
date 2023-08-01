using System;
using System.Collections.Generic;
using System.Linq;
using GameScene.Grid.Entities.ItemInteraction.Logic.Orders;
using GameScene.SpecialGridEntities.PickPoints;
using UnityEngine;

namespace GameScene.Grid.Entities.ItemInteraction.Logic
{
    [CreateAssetMenu(fileName = "ItemOrder", menuName = "Item/TypeOrder")]
    public class InteractableItemTypeStackOrder : AbstractInteractableItemOrder
    {
        // Order in which the items may be stacked: a > b > c > d
        public List<InteractableItemType> orderFromBottomToTop;

        // Allow stacking suborders like b > c > d
        public bool allowDelayedStart;

        // Allow skipping items like a > c > d
        public bool allowSkipsAfterFirstItem;

        public override bool IsFeasibleOrder(List<InteractableItemType> order)
        {
            var eachTypeBelongsToOrder = order.All(type => orderFromBottomToTop.Contains(type));
            if (eachTypeBelongsToOrder)
            {
                var firstItemIsValid = order.Count == 0 || allowDelayedStart || IsBottom(order[0]);
                if (firstItemIsValid)
                {
                    var orderIsCorrect = true;
                    for (int i = 0; i < order.Count - 1; i++)
                    {
                        orderIsCorrect &= AIsToppableByB(order[i], order[i + 1]);
                    }

                    return orderIsCorrect;
                }
            }

            return false;
        }

        private bool AIsToppableByB(InteractableItemType typeA, InteractableItemType typeB)
        {
            var indexOfA = orderFromBottomToTop.IndexOf(typeA);
            var indexOfB = orderFromBottomToTop.IndexOf(typeB);
            if (allowSkipsAfterFirstItem)
            {
                return indexOfB - indexOfA > 0;
            }

            return indexOfB - indexOfA == 1;
        }

        private bool IsBottom(InteractableItemType type)
        {
            return type == orderFromBottomToTop[0];
        }

        private bool IsTop(InteractableItemType type)
        {
            return type == orderFromBottomToTop[^1];
        }
    }
}