using System.Collections.Generic;
using System.Linq;
using GameScene.Grid.Entities.ItemInteraction.Logic;
using GameScene.SpecialGridEntities.PickPoints;
using UnityEngine;

namespace GameScene.Grid.Entities.ItemInteraction.Stackable
{
    public class OrderedStack : InteractableItem
    {
        [SerializeField] private List<InteractableItem> stack = new();
        [SerializeField] private AbstractInteractableItemOrder order;

        private void Start()
        {
            if (null == order)
            {
                Debug.LogError("Order of stack is not defined!");
            }
        }

        public override bool CanBeToppedWithItem(InteractableItem item)
        {
            var checkList = new List<InteractableItemType>();
            checkList.Add(GetItemType());
            checkList.AddRange(stack.Select(item => item.GetItemType()));
            checkList.Add(item.GetItemType());

            return order.IsFeasibleOrder(checkList);
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

        public override bool IsComplete()
        {
            return false;
        }
    }
}