using System.Collections.Generic;
using Code.GameScene.Inventory;
using Code.GameScene.Items.Item;
using DG.Tweening;
using UnityEngine;

namespace Code.GameScene.Items.Field
{
    public class FieldSpotWrapperInstance : MonoBehaviour
    {
        public const float SpotWidth = 100;
        public const float SpotHeight = 70;
        public const int MaxNumberOfRenderers = 170;

        public FieldSpotInstance spotInstancePrefab;

        public FieldSpotInstance[] fieldSpotInstances;
        public FieldSpotInstance[,] fieldSpotGrid;

        public Sequence BlendInFieldGrid(int rows, int columns)
        {
            InstantBlendOutAllSpots();

            fieldSpotGrid = new FieldSpotInstance[rows, columns];

            var sequence = DOTween.Sequence();
            for (int row = 0; row < rows; row++)
            {
                for (int column = 0; column < columns; column++)
                {
                    FieldSpotInstance fieldSpotInstance = fieldSpotInstances[columns * row + column];

                    var pos = GetSpotPosition(row, rows, column, columns);
                    fieldSpotInstance.transform.position = pos;
                    fieldSpotInstance.InstantUpdateFieldSpot(null);
                    fieldSpotInstance.SetRowAndColumn(row, column);
                    fieldSpotGrid[row, column] = fieldSpotInstance;
                    sequence.Insert(0, fieldSpotInstance.BlendIn().SetDelay(0.5f + (columns * row + column) * 0.01f));
                }
            }

            return sequence;
        }

        private void InstantBlendOutAllSpots()
        {
            foreach (var fieldSpotRenderer in fieldSpotInstances)
            {
                fieldSpotRenderer.InstantBlendOut();
            }
        }

        private Sequence BlendOutAllSpots()
        {
            Sequence sequence = DOTween.Sequence();
            foreach (var fieldSpotRenderer in fieldSpotInstances)
            {
                sequence.Insert(0, fieldSpotRenderer.BlendOut());
            }

            return sequence;
        }

        private Vector3 GetSpotPosition(int row, int rows, int column, int columns)
        {
            Vector3 currentPosition = transform.position;
            float positionY = currentPosition.y + row * SpotHeight;
            float offsetY = -(SpotHeight * (rows - 1)) / 2;
            float positionX = currentPosition.x + column * SpotWidth;
            float offsetX = -(SpotWidth * (columns - 1)) / 2;
            return new Vector3(positionX + offsetX, positionY + offsetY, row);
        }

        private FieldSpotInstance[] InitFieldSpotInstances(int maxNumberOfRenderers)
        {
            FieldSpotInstance[] result = new FieldSpotInstance[maxNumberOfRenderers];
            for (int i = 0; i < maxNumberOfRenderers; i++)
            {
                result[i] = Instantiate(spotInstancePrefab, transform);
            }

            return result;
        }

        public void SetGridInstance(FieldGridInstance fieldGridInstance)
        {
            fieldSpotInstances = InitFieldSpotInstances(MaxNumberOfRenderers);
            foreach (var fieldSpotInstance in fieldSpotInstances)
            {
                fieldSpotInstance.SetGridInstance(fieldGridInstance);
            }
        }

        // // Rotate the whole Field by 90 degrees
        // public void RotateLeft()
        // {
        //     FieldSpot[,] newSpots = new FieldSpot[Columns, Rows];
        //
        //     for (int row = 0; row < Rows; row++)
        //     {
        //         for (int column = 0; column < Columns; column++)
        //         {
        //             newSpots[column, row] = _fieldSpots[row, Columns - column];
        //         }
        //     }
        //
        //     (Rows, Columns) = (Columns, Rows);
        //
        //     _fieldSpots = newSpots;
        //
        //     spotWrapperInstance.UpdateFieldGrid(_fieldSpots);
        // }
        //
        // // Rotate the whole Field by -90 degrees
        // public void RotateRight()
        // {
        //     FieldSpot[,] newSpots = new FieldSpot[Columns, Rows];
        //
        //     for (int row = 0; row < Rows; row++)
        //     {
        //         for (int column = 0; column < Columns; column++)
        //         {
        //             newSpots[column, row] = _fieldSpots[Rows - row, column];
        //         }
        //     }
        //
        //     (Rows, Columns) = (Columns, Rows);
        //
        //     _fieldSpots = newSpots;
        //
        //     spotWrapperInstance.UpdateFieldGrid(_fieldSpots);
        // }

        public bool IsFreeAt(int row, int column)
        {
            if (row < 0 || column < 0 || row >= fieldSpotGrid.GetLength(0) || column >= fieldSpotGrid.GetLength(1))
            {
                return false;
            }

            return fieldSpotGrid[row, column].IsFree();
        }

        public void SetUpItemAt(int row, int column, FieldEntityData selectedItemData)
        {
            fieldSpotGrid[row, column].UpdateFieldSpot(selectedItemData);
        }

        public bool CanHarvest(int row, int column)
        {
            return fieldSpotGrid[row, column].CanHarvest();
        }

        public Dictionary<InventoryItemType,int> GetHarvest(int row, int column)
        {
            return fieldSpotGrid[row, column].GetHarvest();
        }
    }
}