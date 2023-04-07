using DG.Tweening;
using GameScene.Items.Item;
using UnityEngine;

namespace GameScene.Items.Field
{
    public class FieldSpotWrapperInstance : MonoBehaviour
    {
        public const float SpotWidth = 120;
        public const float SpotHeight = 90;

        public FieldSpotInstance spotInstancePrefab;

        public FieldSpotInstance[] fieldSpotInstances;
        public FieldSpotInstance[,] fieldSpotGrid;

        private bool inEvolution;

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
                    fieldSpotGrid[row, column] = fieldSpotInstance;
                    sequence.Insert(0, fieldSpotInstance.BlendIn().SetDelay(0.5f + (row + column) * 0.05f));
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

        private FieldSpotInstance[] InitFieldSpotInstances(int numberOfRenderers)
        {
            FieldSpotInstance[] result = new FieldSpotInstance[numberOfRenderers];
            for (int i = 0; i < numberOfRenderers; i++)
            {
                result[i] = Instantiate(spotInstancePrefab, transform);
            }

            return result;
        }

        public void SetGridInstance(FieldGridInstance fieldGridInstance)
        {
            fieldSpotInstances = InitFieldSpotInstances(fieldGridInstance.Rows * fieldGridInstance.Columns);
            foreach (var fieldSpotInstance in fieldSpotInstances)
            {
                fieldSpotInstance.SetGridInstance(fieldGridInstance);
            }
        }

        public bool IsFreeAt(int row, int column)
        {
            if (row < 0 || column < 0 || row >= fieldSpotGrid.GetLength(0) || column >= fieldSpotGrid.GetLength(1))
            {
                return false;
            }

            return fieldSpotGrid[row, column].IsFree();
        }

        public void SetUpItemAt(int row, int column, PlantData selectedItemData)
        {
            fieldSpotGrid[row, column].UpdateFieldSpot(selectedItemData);
        }

        public bool CanHarvest(int row, int column)
        {
            return fieldSpotGrid[row, column].CanHarvest();
        }

        public bool InEvolution()
        {
            return inEvolution;
        }
    }
}