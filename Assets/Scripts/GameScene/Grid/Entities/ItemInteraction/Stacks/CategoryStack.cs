using System.Collections.Generic;
using System.Linq;
using GameScene.SpecialGridEntities.PickPoints;
using UnityEngine;

namespace GameScene.Grid.Entities.ItemInteraction.Stackable
{
    // A simple stack can be stacked with anything
    public class CategoryStack : AbstractStack
    {
        [SerializeField] private InteractableItemCategory category;

        private void Start()
        {
            if (null == category)
            {
                Debug.LogError("Category of stack is not defined!");
            }
        }

        public override bool CanBeToppedWithItem(InteractableItem item)
        {
            var checkList = new List<InteractableItemType>();
            checkList.Add(GetItemType());
            checkList.AddRange(stack.Select(stackElement => stackElement.GetItemType()));
            checkList.Add(item.GetItemType());

            return checkList
                .Select(listElement => listElement.category)
                .All(itemCat => itemCat == category);
        }

        public override bool IsComplete()
        {
            return false;
        }
    }
}