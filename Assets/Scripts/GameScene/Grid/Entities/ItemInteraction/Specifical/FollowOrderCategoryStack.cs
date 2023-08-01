using System.Collections.Generic;
using System.Linq;
using GameScene.Grid.Entities.ItemInteraction.Logic;
using GameScene.Grid.Entities.ItemInteraction.Logic.Orders;
using GameScene.Grid.Entities.ItemInteraction.Stackable;
using GameScene.SpecialGridEntities.PickPoints;
using UnityEngine;

namespace GameScene.Grid.Entities.ItemInteraction.Specifical
{
    public class FollowOrderCategoryStack : CategoryStack
    {
        [SerializeField] private AbstractInteractableItemOrder orderForCompletion;
        
        public override bool IsComplete()
        {
            var checkList = new List<InteractableItemType>();
            checkList.Add(GetItemType());
            checkList.AddRange(stack.Select(stackElement => stackElement.GetItemType()));

            return orderForCompletion.IsFeasibleOrder(checkList);
        }
    }
}