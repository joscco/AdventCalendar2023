using System.Collections.Generic;
using System.Linq;
using GameScene.SpecialGridEntities.PickPoints;
using UnityEngine;

namespace GameScene.Grid.Entities.ItemInteraction.Logic
{
    [CreateAssetMenu(fileName = "ItemOrder", menuName = "Item/CategoryOrder")]
    public class InteractableItemCategoryStackOrder : AbstractInteractableItemOrder
    {
        // Order in which the items may be stacked: a > b > c > d
        public List<InteractableItemCategory> orderFromBottomToTop;

        // Allow stacking suborders like b > c > d
        public bool allowDelayedStart;

        // Allow skipping items like a > c > d
        public bool allowSkipsAfterFirstItem;

        public override bool IsFeasibleOrder(List<InteractableItemType> order)
        {
            var eachTypeBelongsToOrder = order.All(type => orderFromBottomToTop.Contains(type.category));
            if (eachTypeBelongsToOrder)
            {
                var firstItemIsValid = order.Count == 0 || allowDelayedStart || IsBottom(order[0].category);
                if (firstItemIsValid)
                {
                    var orderIsCorrect = true;
                    for (int i = 0; i < order.Count - 1; i++)
                    {
                        orderIsCorrect &= AIsToppableByB(order[i].category, order[i + 1].category);
                    }

                    return orderIsCorrect;
                }
            }

            return false;
        }

        private bool AIsToppableByB(InteractableItemCategory categoryA, InteractableItemCategory categoryB)
        {
            var indexOfA = orderFromBottomToTop.IndexOf(categoryA);
            var indexOfB = orderFromBottomToTop.IndexOf(categoryB);
            if (allowSkipsAfterFirstItem)
            {
                return indexOfB - indexOfA > 0;
            }

            return indexOfB - indexOfA == 1;
        }

        private bool IsBottom(InteractableItemCategory category)
        {
            return category == orderFromBottomToTop[0];
        }
    }
}