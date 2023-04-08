using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using GameScene.Inventory.Renderer;
using GameScene.Items.Item;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameScene.Inventory
{
    public class SampleList : MonoBehaviour
    {
        [SerializeField] private SampleSlotWrapper slotWrapper;


        private SampleSlot _selectedSlot;
        
        private List<ItemType> _sampleTypes;

        private void Start()
        {
            _sampleTypes = new List<ItemType>();
            slotWrapper.SetSampleList(this);
        }

        public void SelectItemSlot(SampleSlot slot)
        {
            SampleSlot previouslySelectedSlot = _selectedSlot;
            DeselectItemSlot();
            
            if (previouslySelectedSlot != slot)
            {
                slotWrapper.UpsizeSlot(slot);
                _selectedSlot = slot;
            }
        }

        public void DeselectItemSlot()
        {
            _selectedSlot = null;
            slotWrapper.DownsizeAllSlots();
        }

        public void SetSampleTypes(List<ItemType> sampleTypes)
        {
            _sampleTypes = sampleTypes;
            var data = _sampleTypes.Select(type => Level.Get().itemWiki.GetItemDataForType(type)).ToList();
            slotWrapper.UpdateSlots(data);
        }

        public SampleSlot GetSelectedSlot()
        {
            return _selectedSlot;
        }
    }
}