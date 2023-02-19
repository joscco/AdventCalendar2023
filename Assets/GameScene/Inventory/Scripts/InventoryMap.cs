using System;
using System.Collections.Generic;

namespace GameScene.Inventory.Scripts
{
    public class InventoryMap
    {
        private readonly int _maxSlots = 15;
        private readonly int _maxItemsPerSlot = 99;
        
        private readonly Dictionary<InventoryItem, int> _inventoryItems;

        public InventoryMap()
        {
            _inventoryItems = new Dictionary<InventoryItem, int>();
        }
        
        public InventoryMap Copy()
        {
            var copy = new InventoryMap();
            foreach (var key in _inventoryItems.Keys)
            {
                copy._inventoryItems.Add(key, _inventoryItems[key]);
            }

            return copy;
        }

        public int GetSlotsNeeded()
        {
            return GetAsSlots().Count;
        }

        public List<InventorySlotEntry> GetAsSlots()
        {
            List<InventorySlotEntry> result = new List<InventorySlotEntry>();
            foreach (var key in _inventoryItems.Keys)
            {
                int amount = _inventoryItems[key];
                while (amount > 0)
                {
                    result.Add(new InventorySlotEntry(key, Math.Clamp(amount, 0, _maxItemsPerSlot)));
                    amount -= _maxItemsPerSlot;
                }
            }
            return result;
        }
        
        public int GetItemCount(InventoryItem item)
        {
            return _inventoryItems.ContainsKey(item) ? _inventoryItems[item] : 0;
        }

        public void RemoveItems(InventoryItem item, int n)
        {
            if (_inventoryItems.ContainsKey(item))
            {
                if (_inventoryItems[item] <= n)
                {
                    _inventoryItems.Remove(item);
                }
                else
                {
                    _inventoryItems[item] -= n;
                }
            }
        }

        public void AddItems(InventoryItem item, int n)
        {
            if (_inventoryItems.ContainsKey(item))
            {
                _inventoryItems[item] += n;
            }
            else
            {
                _inventoryItems.Add(item, n);
            }
        }

        public int GetMaxNumberOfSlots()
        {
            return _maxSlots;
        }
    }
}