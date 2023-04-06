using System.Collections.Generic;
using Code.GameScene.Inventory;
using Code.GameScene.Items.Item;
using DG.Tweening;
using GameScene;
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
            Level.Get().levelData.RemoveAction();
            if (Level.Get().levelData.NewDayHasStarted())
            {
                spotWrapperInstance.EvolveField();
            }
        }

        public void OnSpotClick(int row, int column)
        {
            Debug.Log("Spot " + row + "/" + column + " was clicked.");
            if (Level.Get().levelData.HasActionsLeft()
                && !spotWrapperInstance.InEvolution()
                && spotWrapperInstance.IsFreeAt(row, column)
                && Level.Get().inventory.GetSelectedItem() != null)
            {
                var selectedItemData = Level.Get().inventory.GetSelectedItem();
                spotWrapperInstance.SetUpItemAt(row, column, selectedItemData);

                Level.Get().inventory.RemoveInventoryItems(selectedItemData.plantType, 1);

                if (Level.Get().inventory.GetSelectedSlot().GetCount() <= 0)
                {
                    Level.Get().inventory.SelectItemSlot(null);
                }

                SubtractAction();
            }
        }

        public void OnFieldEntityClick(int row, int column)
        {
            Debug.Log("Field Entity " + row + "/" + column + " was clicked.");
            if (Level.Get().levelData.HasActionsLeft()
                && !spotWrapperInstance.InEvolution()
                && spotWrapperInstance.CanHarvest(row, column))
            {
                Dictionary<PlantType, int> harvest = spotWrapperInstance.GetHarvest(row, column);
                foreach (var harvestType in harvest.Keys)
                {
                    Level.Get().inventory.AddInventoryItems(harvestType, harvest[harvestType]);
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