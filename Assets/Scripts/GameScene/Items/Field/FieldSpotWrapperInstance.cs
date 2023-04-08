using System.Collections.Generic;
using DG.Tweening;
using GameScene.Items.Item;
using UnityEngine;

namespace GameScene.Items.Field
{
    public class FieldSpotWrapperInstance : MonoBehaviour
    {

        public FieldSpot spotPrefab;

        public FieldSpot[] fieldSpotInstances;
        public FieldSpot[,] fieldSpotGrid;

        public Sequence BlendInFieldGrid(int rows, int columns)
        {
            InstantBlendOutAllSpots();

            fieldSpotGrid = new FieldSpot[rows, columns];

            var sequence = DOTween.Sequence();
            for (int row = 0; row < rows; row++)
            {
                for (int column = 0; column < columns; column++)
                {
                    FieldSpot fieldSpot = fieldSpotInstances[columns * row + column];

                    var pos = GetSpotPosition(row, rows, column, columns);
                    fieldSpot.transform.position = pos;
                    fieldSpot.InstantUpdateFieldEntity(null);
                    fieldSpotGrid[row, column] = fieldSpot;
                    sequence.Insert(0, fieldSpot.BlendIn(0.5f + (row + column) * 0.05f));
                }
            }

            return sequence;
        }
        
        public void BlendInRandomItems(int rows, int columns, List<ItemData> randomData)
        {
            for (int row = 0; row < rows; row++)
            {
                for (int column = 0; column < columns; column++)
                {
                    FieldSpot fieldSpot = fieldSpotGrid[row, column];
                    ItemData data = randomData[Random.Range(0, randomData.Count)];
                    fieldSpot.UpdateFieldEntity(data, 0.5f + (row + column) * 0.05f);
                }
            }

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
                    FieldSpot fieldSpot = fieldSpotGrid[row, column];
                    sequence.Insert(0, fieldSpot.BlendOut().SetDelay(0.5f + (row + column) * 0.05f));
                }
            }

            return sequence;
        }

        private Vector3 GetSpotPosition(int row, int rows, int column, int columns)
        {
            Vector3 currentPosition = transform.position;
            float positionY = currentPosition.y + row * FieldSpot.SpotHeight;
            float offsetY = -(FieldSpot.SpotHeight * (rows - 1)) / 2;
            float positionX = currentPosition.x + column * FieldSpot.SpotWidth;
            float offsetX = -(FieldSpot.SpotWidth * (columns - 1)) / 2;
            return new Vector3(positionX + offsetX, positionY + offsetY, row);
        }

        private FieldSpot[] InitFieldSpotInstances(int numberOfRenderers)
        {
            FieldSpot[] result = new FieldSpot[numberOfRenderers];
            for (int i = 0; i < numberOfRenderers; i++)
            {
                result[i] = Instantiate(spotPrefab, transform);
            }

            return result;
        }

        public void SetGridInstance(FieldGridInstance fieldGridInstance)
        {
            fieldSpotInstances = InitFieldSpotInstances(fieldGridInstance.rows * fieldGridInstance.columns);
        }

        public void SetUpItemAt(int row, int column, ItemData selectedItemData, float animationDelay)
        {
            fieldSpotGrid[row, column].UpdateFieldEntity(selectedItemData, animationDelay);
        }

        public bool CanHarvest(int row, int column)
        {
            return fieldSpotGrid[row, column].CanHarvest();
        }
    }
}