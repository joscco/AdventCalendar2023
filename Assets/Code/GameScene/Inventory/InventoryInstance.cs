using System;
using Code.GameScene.Inventory.Renderer;
using Code.GameScene.Items.Item;
using DG.Tweening;
using UnityEngine;

namespace Code.GameScene.Inventory
{
    public class InventoryInstance : MonoBehaviour
    {
        [SerializeField]
        private InventorySlotWrapperInstance slotWrapperInstance;

        private InventoryMap _inventoryMap;

        private InventorySlotInstance _selectedSlot;

        private void Start()
        {
            _inventoryMap = new InventoryMap();
            slotWrapperInstance.SetInventory(this);
            AddStartItems();
        }

        public void SelectItemSlot(InventorySlotInstance slot)
        {
            InventorySlotInstance previouslySelectedSlot = _selectedSlot;
            DeselectItemSlot();
            
            if (previouslySelectedSlot != slot)
            {
                slotWrapperInstance.UpsizeSlot(slot);
                _selectedSlot = slot;
            }
        }

        public void DeselectItemSlot()
        {
            _selectedSlot = null;
            slotWrapperInstance.DownsizeAllSlots();
        }

        private void AddStartItems()
        {
            var sequence = DOTween.Sequence();
            sequence.AppendInterval(0.5f);
            sequence.AppendCallback(() =>
            {
                AddInventoryItems(PlantType.Daisies, 1);
                AddInventoryItems(PlantType.Roses, 1);
                slotWrapperInstance.UpdateSlots(_inventoryMap.GetSlots());
            });
            sequence.Play();
        }

        public void AddInventoryItems(PlantType itemType, int amount)
        {
            _inventoryMap.AddItems(itemType, amount);
            slotWrapperInstance.UpdateSlots(_inventoryMap.GetSlots());
        }

        public void RemoveInventoryItems(PlantType itemType, int amount)
        {
            _inventoryMap.RemoveItems(itemType, amount);
            slotWrapperInstance.UpdateSlots(_inventoryMap.GetSlots());
        }

        public PlantData GetSelectedItem()
        {
            if (_selectedSlot == null)
            {
                return null;
            }

            if (_selectedSlot.GetCount() <= 0)
            {
                DeselectItemSlot();
                return null;
            }
            
            return Main.Get().plantWiki.GetPlantDataForPlant(_selectedSlot.GetItemType());
        }

        public InventorySlotInstance GetSelectedSlot()
        {
            return _selectedSlot;
        }
    }
}