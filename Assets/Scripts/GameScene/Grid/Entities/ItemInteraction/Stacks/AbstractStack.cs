using System.Collections.Generic;
using UnityEngine;

namespace GameScene.Grid.Entities.ItemInteraction.Stackable
{
    public abstract class AbstractStack : InteractableItem
    {
        [SerializeField] protected List<InteractableItem> stack = new();

        public override InteractableItem GetItem()
        {
            if (stack.Count > 0)
            {
                return stack[^1];
            }

            return null;
        }

        protected override void OnTopWithItem(InteractableItem item)
        {
            if (IsBearingItem())
            {
                item.AttachToPickupPoint(stack[^1].GetItemHolder());
            }
            else
            {
                item.AttachToPickupPoint(GetItemHolder());
            }

            stack.Add(item);
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
    }
}