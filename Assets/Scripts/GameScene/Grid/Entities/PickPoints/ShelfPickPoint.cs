using System.Collections.Generic;
using System.Linq;
using General.Grid;
using Levels.WizardLevel;
using UnityEngine;

namespace GameScene.SpecialGridEntities.PickPoints
{
    public class ShelfPickPoint : PickPoint
    {
        [SerializeField] private List<PickableItemHolder> pickableHolders;

        public override bool CanTakeItem(PickableItem item)
        {
            return pickableHolders.Any(holder => !holder.HasItem());
        }

        public override PickableItem GetItem()
        {
            return pickableHolders.First(holder => holder.HasItem()).GetItem();
        }

        public override void GiveItem(PickableItem item)
        {
            var firstFreeHolder = pickableHolders.First(holder => !holder.HasItem());
            firstFreeHolder.SetItem(item);
            item.AttachToPickupPoint(firstFreeHolder.GetPickupPoint());
            item.SetSortingOrder(-currentMainIndex.y);
        }

        public override bool HasItemToGive()
        {
            return pickableHolders.Any(holder => holder.HasItem());
        }

        public override void RemoveItem(PickableItem item)
        {
            var holder = pickableHolders.First(holder => holder.GetItem() == item);
            holder.SetItem(null);
        }

        public override bool IsComplete()
        {
            return false;
        }

        protected override void UpdateSortingOrder(int newOrder)
        {
            entityRenderer.SetSortingOrder(newOrder);
            pickableHolders.ForEach(holder => holder.GetItem()?.SetSortingOrder(newOrder));
        }
    }
}