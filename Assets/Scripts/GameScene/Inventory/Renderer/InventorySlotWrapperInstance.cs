using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace GameScene.Inventory.Renderer
{
    public class InventorySlotWrapperInstance : MonoBehaviour
    {
        public InventorySlotInstance slotInstancePrefab;
        private SpriteRenderer _outlineRenderer;

        private const int SlotWidth = 108;
        private const int SlotMargin = 30;
        private const int MinWidthOutlineContainer = 0;
        private const int SideOffsetContainer = 70;
        
        // All Slots, no matter if available or not
        private InventorySlotInstance[] _slotRenderers;
        
        // Differentiated by whether active or not
        private List<InventorySlotInstance> _activeSlotRenderers;
        private List<InventorySlotInstance> _inactiveSlots;

        private void Start()
        {
            _outlineRenderer = GetComponent<SpriteRenderer>();
            _outlineRenderer.size = new Vector2(0, _outlineRenderer.size.y);

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
            var tweenSequence = DOTween.Sequence();
            
            var currentActiveSlotRendererIndex = 0;
            var currentSlotInfoIndex = 0;

            const float BLEND_OUT_TIME = 0;
            const float RESIZE_TIME = 0.3f;
            const float BLEND_IN_TIME = 0.5f;

            List<InventorySlotInstance> newActiveSlotRenderers = new List<InventorySlotInstance>();

            while (currentSlotInfoIndex < newSlotList.Count)
            {
                InventorySlotInstance currentActiveSlotInstance = null;
                if (currentActiveSlotRendererIndex < _activeSlotRenderers.Count)
                {
                    currentActiveSlotInstance = _activeSlotRenderers[currentActiveSlotRendererIndex];
                }
                InventorySlot newSlotInfo = newSlotList[currentSlotInfoIndex];

                if (null == currentActiveSlotInstance)
                {
                    // No Active Renderers left. Take a deactivated one. Blend in
                    currentActiveSlotInstance = PopInactiveSlotRenderer();
                    tweenSequence.Insert(BLEND_IN_TIME, currentActiveSlotInstance.UpdateSlot(newSlotInfo));
                    newActiveSlotRenderers.Add(currentActiveSlotInstance);
                    
                    currentActiveSlotRendererIndex++;
                    currentSlotInfoIndex++;
                }
                else
                {
                    if (currentActiveSlotInstance.GetItemType() == newSlotInfo.ItemType)
                    {
                        // Renderer fits well, use it but update number if necessary
                        tweenSequence.Insert(BLEND_IN_TIME, currentActiveSlotInstance.UpdateSlot(newSlotInfo));
                        newActiveSlotRenderers.Add(currentActiveSlotInstance);
                        
                        currentActiveSlotRendererIndex++;
                        currentSlotInfoIndex++;
                    } 
                    else if (currentActiveSlotInstance.GetItemType() < newSlotInfo.ItemType)
                    {
                        // Current Renderer holds old value that is not needed anymore. Deactivate it
                        // Increase only renderer index and try again with same slot info
                        tweenSequence.Insert(BLEND_OUT_TIME, currentActiveSlotInstance.UpdateSlot(null));
                        _inactiveSlots.Add(currentActiveSlotInstance);
                        
                        currentActiveSlotRendererIndex++;
                    }
                    else
                    {
                        // Current Renderer holds a more advanced value than the new slot info.
                        // We either need a new renderer
                        // Do not increase the renderer index but continue
                        currentActiveSlotInstance = PopInactiveSlotRenderer();
                        tweenSequence.Insert(BLEND_IN_TIME, currentActiveSlotInstance.UpdateSlot(newSlotInfo));
                        newActiveSlotRenderers.Add(currentActiveSlotInstance);

                        currentSlotInfoIndex++;
                    }
                }

            }

            // Deactivate all remaining active Slots.
            for (int i = currentActiveSlotRendererIndex; i < _activeSlotRenderers.Count; i++)
            {
                InventorySlotInstance currentActiveSlotInstance = _activeSlotRenderers[i];
                tweenSequence.Insert(BLEND_OUT_TIME, currentActiveSlotInstance.UpdateSlot(null));
                _inactiveSlots.Add(currentActiveSlotInstance);
            }

            _activeSlotRenderers = newActiveSlotRenderers;

            tweenSequence.Insert(RESIZE_TIME, ResizeSlotContainer(_activeSlotRenderers.Count));
            tweenSequence.Insert(RESIZE_TIME, RepositionSlots(_activeSlotRenderers));
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
        
        private Tween RepositionSlots(List<InventorySlotInstance> activeSlots)
        {
            var sequence = DOTween.Sequence();
            float offsetLeft = 50 - (activeSlots.Count * (SlotWidth + SlotMargin) - SlotMargin) / 2;
            for (int i = 0; i < _activeSlotRenderers.Count; i++)
            {
                InventorySlotInstance slotInstance = _activeSlotRenderers[i];
                float newX = offsetLeft + i * (SlotWidth + SlotMargin);
                sequence.Insert(0, slotInstance.transform
                    .DOMoveX(newX, 0.5f)
                    .SetEase(Ease.InOutBack));
            }

            return sequence;
        }
        
        private Tween ResizeSlotContainer(int numberOfActiveSlots)
        {
            float newWidth = 0;
            if (numberOfActiveSlots > 0)
            {
                newWidth = SideOffsetContainer + numberOfActiveSlots * (SlotWidth + SlotMargin) - SlotMargin;
            }
            
            Vector2 newSize = new Vector2(Math.Max(MinWidthOutlineContainer, newWidth), _outlineRenderer.size.y);
            return DOTween.To(() => _outlineRenderer.size,
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
            foreach (var slotInstance in _slotRenderers)
            {
                slotInstance.SetInventory(instance);
            }
        }
    }
}