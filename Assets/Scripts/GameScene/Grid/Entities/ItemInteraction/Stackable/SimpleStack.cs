using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameScene.Grid.Entities.ItemInteraction.Stackable
{
    // A simple stack can be stacked with anything
    public class SimpleStack : InteractableItem
    {
        [SerializeField] private int maxNumberOfItemsOnTop = 9;
        [SerializeField] private List<InteractableItem> stack = new();
        
        public override bool CanBeToppedWithItem(InteractableItem item)
        {
            return stack.Count < maxNumberOfItemsOnTop;
        }

        public override InteractableItem GetItem()
        {
            if (stack.Count > 0)
            {
                return stack[^1];
            }

            return null;
        }

        public override void TopWithItem(InteractableItem item)
        {
            if (IsBearingItem())
            {
                item.AttachToPickupPoint(stack[^1].GetItemHolder());
            }
            else
            {
                item.AttachToPickupPoint(GetItemHolder());
            }

            stack.Append(item);
        }

        public override bool IsBearingItem()
        {
            return stack.Count > 0;
        }

        public override void RemoveItem(InteractableItem item)
        {
            if (stack.Count > 0)
            {
                stack.RemoveAt(stack.Count - 1);
            }
        }

        public override bool IsComplete()
        {
            return false;
        }
    }
}