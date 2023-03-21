using System.Collections.Generic;
using Code.GameScene.Inventory;
using Code.GameScene.Items.Item;
using DG.Tweening;
using UnityEngine;
using Random = Unity.Mathematics.Random;

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
                    sequence.Insert(0, fieldSpotInstance.BlendIn().SetDelay(0.5f + (row + column) * 0.05f));
                }
            }

            // var newSequence = DOTween.Sequence();
            // newSequence
            //     .AppendInterval(5f)
            //     .AppendCallback(() => Rotate(true))
            //     .AppendInterval(5f)
            //     .AppendCallback(() => Rotate(false))
            //     .AppendInterval(5f)
            //     .AppendCallback(() => Rotate(false));

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
            int rows = fieldSpotGrid.GetLength(0);
            int columns = fieldSpotGrid.GetLength(1);
            for (int row = 0; row < rows; row++)
            {
                for (int column = 0; column < columns; column++)
                {
                    FieldSpotInstance fieldSpotInstance = fieldSpotGrid[row, column];
                    sequence.Insert(0, fieldSpotInstance.BlendOut().SetDelay(0.5f + (row + column) * 0.05f));
                }
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

        public Tween Rotate(bool left)
        {
            var sequence = DOTween.Sequence();

            int rows = fieldSpotGrid.GetLength(0);
            int columns = fieldSpotGrid.GetLength(1);
            
            int newRows = columns;
            int newColumns = rows;

            var newSpotGrid = new FieldSpotInstance[newRows, newColumns];

            for (int row = 0; row < rows; row++)
            {
                for (int column = 0; column < columns; column++)
                {
                    int newRow = left ? (columns - 1 - column) : column;
                    int newColumn = left ? row : (rows - 1 - row);

                    FieldSpotInstance fieldSpotInstance = fieldSpotGrid[row, column];
                    
                    var pos = GetSpotPosition(newRow, newRows, newColumn, newColumns);
                    sequence.Insert(1f, fieldSpotInstance.DoMoveTo(pos));
                    fieldSpotInstance.SetRowAndColumn(newRow, newColumn);
                    newSpotGrid[newRow, newColumn] = fieldSpotInstance;
                }
            }

            fieldSpotGrid = newSpotGrid;

            return sequence;
        }

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