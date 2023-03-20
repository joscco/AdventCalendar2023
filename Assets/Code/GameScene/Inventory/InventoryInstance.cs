using Code.GameScene.Inventory.Renderer;
using Code.GameScene.Items.Item;
using UnityEngine;

namespace Code.GameScene.Inventory
{
    public class InventoryInstance : MonoBehaviour
    {
        [SerializeField]
        private InventorySlotWrapperInstance slotWrapperInstance;

        [SerializeField] 
        private InventoryItemWiki wiki;
        
        private InventoryMap _inventoryMap;

        private InventorySlotInstance _selectedSlot;

        private void Start()
        {
            _inventoryMap = new InventoryMap();
            slotWrapperInstance.SetInventory(this);
            slotWrapperInstance.SetWiki(wiki);
            AddStartItems();
        }

        public void SelectItemSlot(InventorySlotInstance slot)
        {
            Debug.Log("All Slots DeSelected");
            InventorySlotInstance previouslySelectedSlot = _selectedSlot;
            DeselectItemSlot();
            
            if (previouslySelectedSlot != slot)
            {
                Debug.Log("New Slot Selected");
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
            AddInventoryItems(InventoryItemType.Grass, 5);
            slotWrapperInstance.UpdateSlots(_inventoryMap.GetSlots());
        }

        public void AddInventoryItems(InventoryItemType itemType, int amount)
        {
            _inventoryMap.AddItems(itemType, amount);
            slotWrapperInstance.UpdateSlots(_inventoryMap.GetSlots());
        }

        public void RemoveInventoryItems(InventoryItemType itemType, int amount)
        {
            _inventoryMap.RemoveItems(itemType, amount);
            slotWrapperInstance.UpdateSlots(_inventoryMap.GetSlots());
        }

        public FieldEntityData GetSelectedItem()
        {
            if (_selectedSlot != null)
            {
                return wiki.GetFieldEntityDataForItem(_selectedSlot.GetItemType());
            }

            return null;
        }
    }
}