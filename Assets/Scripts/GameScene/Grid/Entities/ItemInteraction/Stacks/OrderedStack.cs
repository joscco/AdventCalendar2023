using System.Collections.Generic;
using System.Linq;
using GameScene.Grid.Entities.ItemInteraction.Logic;
using GameScene.SpecialGridEntities.PickPoints;
using UnityEngine;

namespace GameScene.Grid.Entities.ItemInteraction.Stackable
{
    public class OrderedStack : AbstractStack
    {
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

        public override bool IsComplete()
        {
            return false;
        }
    }
}