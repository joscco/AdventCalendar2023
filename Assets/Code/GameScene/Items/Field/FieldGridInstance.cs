using System.Collections.Generic;
using Code.GameScene.Inventory;
using Code.GameScene.Items.Item;
using DG.Tweening;
using UnityEngine;

namespace Code.GameScene.Items.Field
{
    public class FieldGridInstance : MonoBehaviour
    {
        public int Columns = 20;
        public int Rows = 10;

        public FieldSpotWrapperInstance spotWrapperInstance;

        private void Start()
        {
            spotWrapperInstance.SetGridInstance(this);
            BlendInField();
        }

        private void SubtractAction()
        {
            Main.Get().levelManager.RemoveAction();
            if (Main.Get().levelManager.NewDayHasStarted())
            {
                spotWrapperInstance.EvolveField();
            }
        }

        public void OnSpotClick(int row, int column)
        {
            Debug.Log("Spot " + row + "/" + column + " was clicked.");
            if (Main.Get().levelManager.HasActionsLeft() 
                && !spotWrapperInstance.InEvolution()
                && spotWrapperInstance.IsFreeAt(row, column) 
                && Main.Get().inventory.GetSelectedItem() != null)
            {
                var selectedItemData = Main.Get().inventory.GetSelectedItem();
                spotWrapperInstance.SetUpItemAt(row, column, selectedItemData);
                
                Main.Get().inventory.RemoveInventoryItems(selectedItemData.plantType, 1);

                if (Main.Get().inventory.GetSelectedSlot().GetCount() <= 0)
                {
                    Main.Get().inventory.SelectItemSlot(null);
                }
                
                SubtractAction();
            }
        }

        public void OnFieldEntityClick(int row, int column)
        {
            Debug.Log("Field Entity " + row + "/" + column + " was clicked.");
            if (Main.Get().levelManager.HasActionsLeft() 
                && !spotWrapperInstance.InEvolution()
                && spotWrapperInstance.CanHarvest(row, column))
            {
                Dictionary<PlantType, int> harvest = spotWrapperInstance.GetHarvest(row, column);
                foreach (var harvestType in harvest.Keys)
                {
                    Main.Get().inventory.AddInventoryItems(harvestType, harvest[harvestType]);
                }

                spotWrapperInstance.SetUpItemAt(row, column, null);
                
                SubtractAction();
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