using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Code.GameScene.Inventory.Renderer
{
    public class InventorySlotWrapperInstance : MonoBehaviour
    {
        public InventorySlotInstance slotInstancePrefab;
        private SpriteRenderer _outlineRenderer;

        private const int SlotWidth = 100;
        private const int SlotMargin = 20;
        private const int MinWidthOutlineContainer = 100;
        
        // All Slots, no matter if available or not
        private InventorySlotInstance[] _slotRenderers;
        
        // Differentiated by whether active or not
        private List<InventorySlotInstance> _activeSlotRenderers;
        private List<InventorySlotInstance> _inactiveSlots;

        private InventoryInstance _inventoryInstance;
        
        private void Start()
        {
            _outlineRenderer = GetComponent<SpriteRenderer>();

            _slotRenderers = InitSlots(InventoryMap.MaxSlots);
            
            _inactiveSlots = new List<InventorySlotInstance>();
            _activeSlotRenderers = new List<InventorySlotInstance>();
            
            _inactiveSlots.AddRange(_slotRenderers);
        }
        
        private InventorySlotInstance[] InitSlots(int amount)
        {
            var result = new InventorySlotInstance[amount];
            for (int i = 0; i < amount; i++)
            {
                var slot = Instantiate(slotInstancePrefab, transform);
                slot.InstantUpdateSlot(null);
                result[i] = slot;
            }

            return result;
        }
        
        public void UpdateSlots(List<InventorySlot> newSlotList)
        {
            // Slot Liste folgt immer der gleichen Ordnung -> Vorteil!
            
            var currentActiveSlotRendererIndex = 0;
            var currentSlotInfoIndex = 0;
            
            List<InventorySlotInstance> newActiveSlotRenderers = new List<InventorySlotInstance>();

            while(currentSlotInfoIndex < newSlotList.Count)
            {
                InventorySlotInstance currentActiveSlotInstance = null;
                if (currentActiveSlotRendererIndex < _activeSlotRenderers.Count)
                {
                    currentActiveSlotInstance = _activeSlotRenderers[currentActiveSlotRendererIndex];
                }
                InventorySlot newSlotInfo = newSlotList[currentSlotInfoIndex];

                if (null == currentActiveSlotInstance)
                {
                    // No Active Renderers left. Take a deactivated one
                    currentActiveSlotInstance = PopInactiveSlotRenderer();
                    currentActiveSlotInstance.UpdateSlot(newSlotInfo);
                    newActiveSlotRenderers.Add(currentActiveSlotInstance);
                    
                    currentActiveSlotRendererIndex++;
                    currentSlotInfoIndex++;
                }
                else
                {
                    if (currentActiveSlotInstance.GetItemType() == newSlotInfo.ItemType)
                    {
                        // Renderer fits well, use it but update number if necessary
                        currentActiveSlotInstance.UpdateSlot(newSlotInfo);
                        newActiveSlotRenderers.Add(currentActiveSlotInstance);
                        
                        currentActiveSlotRendererIndex++;
                        currentSlotInfoIndex++;
                    } 
                    else if (currentActiveSlotInstance.GetItemType() < newSlotInfo.ItemType)
                    {
                        // Current Renderer holds old value that is not needed anymore. Deactivate it
                        // Increase only renderer index and try again with same slot info
                        currentActiveSlotInstance.UpdateSlot(null);
                        _inactiveSlots.Add(currentActiveSlotInstance);
                        
                        currentActiveSlotRendererIndex++;
                    }
                    else
                    {
                        // Current Renderer holds a more advanced value than the slot info. We need a new renderer here
                        // Do not increase the renderer index but continue
                        currentActiveSlotInstance = PopInactiveSlotRenderer();
                        currentActiveSlotInstance.UpdateSlot(newSlotInfo);
                        newActiveSlotRenderers.Add(currentActiveSlotInstance);

                        currentSlotInfoIndex++;
                    }
                }

            }

            // Deactivate all remaining active Slots.
            for (int i = currentActiveSlotRendererIndex; i < _activeSlotRenderers.Count; i++)
            {
                InventorySlotInstance currentActiveSlotInstance = _activeSlotRenderers[i];
                currentActiveSlotInstance.UpdateSlot(null);
                _inactiveSlots.Add(currentActiveSlotInstance);
            }

            _activeSlotRenderers = newActiveSlotRenderers;

            ResizeSlotContainer(_activeSlotRenderers.Count);
            RepositionSlots(_activeSlotRenderers);
        }

        private InventorySlotInstance PopInactiveSlotRenderer()
        {
            if (_inactiveSlots.Count == 0)
            {
                throw new Exception("List of inactive Slots is Empty!");
            }
            
            var firstInactiveSlot = _inactiveSlots[0];
            _inactiveSlots.RemoveAt(0);
            return firstInactiveSlot;
        }

        public void InstantUpdateSlots(List<InventorySlot> slots)
        {
            foreach (var inventorySlotRenderer in _slotRenderers)
            {
                inventorySlotRenderer.InstantUpdateSlot(null);
            }

            for (int i = 0; i < slots.Count; i++)
            {
                var inventorySlotRenderer = _slotRenderers[i];
                inventorySlotRenderer.InstantUpdateSlot(slots[i]);
            }
        }
        
        private void RepositionSlots(List<InventorySlotInstance> activeSlots)
        {
            float offsetLeft = 50 - (activeSlots.Count * (SlotWidth + SlotMargin) - SlotMargin) / 2;
            for (int i = 0; i < _activeSlotRenderers.Count; i++)
            {
                InventorySlotInstance slotInstance = _activeSlotRenderers[i];
                float newX = offsetLeft + i * (SlotWidth + SlotMargin);
                slotInstance.transform
                    .DOMoveX(newX, 0.5f)
                    .SetEase(Ease.InOutBack);
            }
        }
        
        private void ResizeSlotContainer(int numberOfActiveSlots)
        {
            float newWidth = 50 + numberOfActiveSlots * (SlotWidth + SlotMargin) - SlotMargin;
            Vector2 newSize = new Vector2(Math.Max(MinWidthOutlineContainer, newWidth), _outlineRenderer.size.y);
            DOTween.To(() => _outlineRenderer.size,
                    (val) => { _outlineRenderer.size = val; },
                    newSize, 
                    0.3f)
                .SetEase(Ease.InOutBack);
        }

        public void UpsizeSlot(InventorySlotInstance slot)
        {
            slot.SizeUp();
        }

        public void DownsizeAllSlots()
        {
            foreach (var activeSlotInstance in _activeSlotRenderers)
            {
                activeSlotInstance.SizeDownToNormal();
            }
        }

        public void SetInventory(InventoryInstance instance)
        {
            _inventoryInstance = instance;
            foreach (var slotInstance in _slotRenderers)
            {
                slotInstance.SetInventory(instance);
            }
        }

        public void SetWiki(InventoryItemWiki wiki)
        {
            foreach (var slotInstance in _slotRenderers)
            {
                slotInstance.SetWiki(wiki);
            }
        }
    }
}