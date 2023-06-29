using System;
using System.Collections.Generic;
using System.Linq;
using GameScene.SpecialGridEntities;
using GameScene.SpecialGridEntities.PickPoints;
using General.Grid;
using Levels.WizardLevel;
using UnityEngine;

namespace GameScene.Grid.Entities.PickPoints
{
    public class HanoiDiskStack : PickPoint
    {
        // Lowest to highest diskHolder

        [SerializeField] private List<PickableItemHolder> holders;

        private readonly PickableItemType[] _diskOrder =
        {
            PickableItemType.L1_HANOI_DISK_XL,
            PickableItemType.L1_HANOI_DISK_L,
            PickableItemType.L1_HANOI_DISK_M,
            PickableItemType.L1_HANOI_DISK_S,
        };

        public override bool CanTakeItem(PickableItem item)
        {
            if (IsHanoiDisk(item.GetItemType()))
            {
                return holders
                    .All(holder => null == holder.GetItem() || FirstIsHigherDiskLevel(holder.GetItem().GetItemType(), item.GetItemType()));
            }

            return false;
        }

        private bool FirstIsHigherDiskLevel(PickableItemType existingType, PickableItemType nextType)
        {
            var existingIndex = Array.IndexOf(_diskOrder, existingType);
            var newIndex = Array.IndexOf(_diskOrder, nextType);
            return newIndex - existingIndex > 0;
        }

        private bool IsHanoiDisk(PickableItemType itemType)
        {
            return _diskOrder.Contains(itemType);
        }

        public override PickableItem GetItem()
        {
            return holders.Last(holder => holder.HasItem()).GetItem();
        }

        public override void GiveItem(PickableItem item)
        {
            // Give to lowest = first
            var firstFreeHolder = holders.First(holder => !holder.HasItem());
            firstFreeHolder.SetItem(item);
            item.AttachToPickupPoint(firstFreeHolder.GetPickupPoint());
        }

        public override bool HasItemToGive()
        {
            return holders.Any(holder => holder.HasItem());
        }

        public override void RemoveItem(PickableItem item)
        {
            // Take from top = last
            var holder = holders.Last(holder => holder.GetItem() == item);
            holder.SetItem(null);
        }

        public override bool IsComplete()
        {
            return holders.All(holder => holder.HasItem());
        }
    }
}