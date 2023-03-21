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
                AddInventoryItems(InventoryItemType.Grass, 1);
                AddInventoryItems(InventoryItemType.Tree, 1);
                slotWrapperInstance.UpdateSlots(_inventoryMap.GetSlots());
            });
            sequence.Play();
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
            if (_selectedSlot == null)
            {
                return null;
            }

            if (_selectedSlot.GetCount() <= 0)
            {
                DeselectItemSlot();
                return null;
            }
            
            return wiki.GetFieldEntityDataForItem(_selectedSlot.GetItemType());
        }

        public InventorySlotInstance GetSelectedSlot()
        {
            return _selectedSlot;
        }
    }
}