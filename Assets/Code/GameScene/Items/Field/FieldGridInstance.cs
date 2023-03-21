using System.Collections.Generic;
using Code.GameScene.Inventory;
using DG.Tweening;
using UnityEngine;

// 2D Field which exists 

namespace Code.GameScene.Items.Field
{
    public class FieldGridInstance : MonoBehaviour
    {
        public int Columns = 20;
        public int Rows = 10;

        public FieldSpotWrapperInstance spotWrapperInstance;

        [SerializeField] private InventoryInstance _inventoryInstance;

        private void Start()
        {
            spotWrapperInstance.SetGridInstance(this);
            BlendInField();
        }

        public void OnSpotClick(int row, int column)
        {
            Debug.Log("Spot " + row + "/" + column + " was clicked.");
            if (spotWrapperInstance.IsFreeAt(row, column) && _inventoryInstance.GetSelectedItem() != null)
            {
                var selectedItemData = _inventoryInstance.GetSelectedItem();
                spotWrapperInstance.SetUpItemAt(row, column, selectedItemData);
                _inventoryInstance.RemoveInventoryItems(selectedItemData.itemType, 1);

                if (_inventoryInstance.GetSelectedSlot().GetCount() <= 0)
                {
                    _inventoryInstance.SelectItemSlot(null);
                }
            }
        }
        
        public void OnFieldEntityClick(int row, int column)
        {
            Debug.Log("Field Entity " + row + "/" + column + " was clicked.");
            if (spotWrapperInstance.CanHarvest(row, column))
            {
                Dictionary<InventoryItemType, int> harvest = spotWrapperInstance.GetHarvest(row, column);
                foreach (var harvestType in harvest.Keys)
                {
                    _inventoryInstance.AddInventoryItems(harvestType, harvest[harvestType]);
                }
                spotWrapperInstance.SetUpItemAt(row, column, null);
            }
        }

        private void BlendInField()
        {
            var sequence = DOTween.Sequence();
            sequence.AppendInterval(0.5f);
            sequence.AppendCallback(() => spotWrapperInstance.BlendInFieldGrid(Rows, Columns));
            sequence.Play();
        }
    }
}