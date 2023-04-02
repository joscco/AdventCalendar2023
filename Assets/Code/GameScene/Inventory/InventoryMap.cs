using System;
using System.Collections.Generic;
using System.Linq;

namespace Code.GameScene.Inventory
{
    public class InventoryMap
    {
        public const int MaxSlots = 10;
        public const int MaxItemsPerSlot = 99;

        private readonly Dictionary<PlantType, int> _inventoryMap;

        public InventoryMap()
        {
            _inventoryMap = new Dictionary<PlantType, int>();
        }

        public InventoryMap Copy()
        {
            var mapCopy = new InventoryMap();
            foreach (var key in _inventoryMap.Keys)
            {
                mapCopy._inventoryMap.Add(key, _inventoryMap[key]);
            }

            return mapCopy;
        }

        public int GetNumberOfSlots()
        {
            return GetSlots().Count;
        }

        public List<InventorySlot> GetSlots()
        {
            List<InventorySlot> result = new List<InventorySlot>();
            List<PlantType> sortedKeys = GetSortedKeys();
            
            foreach (var key in sortedKeys)
            {
                int rawItemAmount = _inventoryMap[key];
                while (rawItemAmount > 0)
                {
                    result.Add(new InventorySlot(key, Math.Clamp(rawItemAmount, 0, MaxItemsPerSlot)));
                    rawItemAmount -= MaxItemsPerSlot;
                }
            }

            return result;
        }

        private List<PlantType> GetSortedKeys()
        {
            return _inventoryMap.Keys.ToList().OrderBy(item => (int)item).ToList();
        }

        public int GetNumberOfItemsOf(PlantType itemType)
        {
            return _inventoryMap.ContainsKey(itemType) ? _inventoryMap[itemType] : 0;
        }

        public void RemoveItems(PlantType itemType, int amount)
        {
            if (_inventoryMap.ContainsKey(itemType))
            {
                if (_inventoryMap[itemType] <= amount)
                {
                    _inventoryMap.Remove(itemType);
                }
                else
                {
                    _inventoryMap[itemType] -= amount;
                }
            }
        }

        public void AddItems(PlantType itemType, int amount)
        {
            if (_inventoryMap.ContainsKey(itemType))
            {
                _inventoryMap[itemType] += amount;
            }
            else
            {
                _inventoryMap.Add(itemType, amount);
            }
        }

        public int GetMaxNumberOfSlots()
        {
            return MaxSlots;
        }
    }
}