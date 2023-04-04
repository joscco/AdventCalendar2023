using System.Collections.Generic;
using System.Linq;
using Code.GameScene.Inventory;
using Code.GameScene.Items.Item;
using DG.Tweening;
using UnityEngine;

namespace Code.GameScene.Items.Field
{
    public class FieldSpotWrapperInstance : MonoBehaviour
    {
        public const float SpotWidth = 120;
        public const float SpotHeight = 90;
        public const int MaxNumberOfRenderers = 170;

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
                    fieldSpotInstance.SetRowAndColumn(row, column);
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

        public Dictionary<PlantType, int> GetHarvest(int row, int column)
        {
            return fieldSpotGrid[row, column].GetHarvest();
        }

        public void EvolveField()
        {
            inEvolution = true;
            var newPlantDataGrid = CalculatePostEvolutionGrid();

            var sequence = DOTween.Sequence();
            sequence.AppendInterval(0.5f);
            sequence.AppendCallback(() => UpdateGridOnEvolution(newPlantDataGrid));
            sequence.AppendCallback(() => { inEvolution = false; });
        }

        private FieldUpdateData[,] CalculatePostEvolutionGrid()
        {
            int gridRows = fieldSpotGrid.GetLength(0);
            int gridColumns = fieldSpotGrid.GetLength(1);
            FieldUpdateData[,] newPlantDataGrid = new FieldUpdateData[gridRows, gridColumns];

            for (int row = 0; row < gridRows; row++)
            {
                for (int column = 0; column < gridColumns; column++)
                {
                    var neighborFieldPositions = GetNeighborPositions(row, column, gridRows, gridColumns);
                    var neighborTypes = neighborFieldPositions
                        .Select(pos => fieldSpotGrid[pos.x, pos.y])
                        .Select(spot => spot.GetPlantData())
                        .Where(data => data != null).ToList();

                    FieldSpotInstance fieldSpot = fieldSpotGrid[row, column];
                    if (fieldSpot.IsFree())
                    {
                        if (neighborTypes.Count > 0 && neighborTypes.Count < 4)
                        {
                            PlantData newData = neighborTypes[Random.Range(0, neighborTypes.Count)];
                            newPlantDataGrid[row, column] = new FieldUpdateData(newData);
                        }
                    }
                    else
                    {
                        if (neighborTypes.Count >= 3)
                        {
                            newPlantDataGrid[row, column] = new FieldUpdateData(false, true);
                        }
                        else
                        {
                            newPlantDataGrid[row, column] = new FieldUpdateData(true, false);
                        }
                    }
                }
            }

            return newPlantDataGrid;
        }

        private void UpdateGridOnEvolution(FieldUpdateData[,] newPlantDataGrid)
        {
            int gridRows = fieldSpotGrid.GetLength(0);
            int gridColumns = fieldSpotGrid.GetLength(1);

            for (int row = 0; row < gridRows; row++)
            {
                for (int column = 0; column < gridColumns; column++)
                {
                    var newData = newPlantDataGrid[row, column];
                    if (newData != null)
                    {
                        if (newData.shouldEvolve)
                        {
                            fieldSpotGrid[row, column].Evolve();
                        }
                        else if (newData.newPlantData)
                        {
                            fieldSpotGrid[row, column].UpdateFieldSpot(newData.newPlantData);
                        }
                        else if (newData.shouldDelete)
                        {
                            fieldSpotGrid[row, column].UpdateFieldSpot(null);
                        }
                    }
                }
            }
        }

        private List<Vector2Int> GetNeighborPositions(int row, int column, int rows, int columns)
        {
            List<Vector2Int> result = new List<Vector2Int>()
            {
                new(row - 1, column - 1),
                new(row, column - 1),
                new(row + 1, column - 1),
                new(row - 1, column),
                new(row, column),
                new(row + 1, column),
                new(row - 1, column + 1),
                new(row, column + 1),
                new(row + 1, column + 1),
            };
            return result.Where(pos => pos.x > 0 && pos.x < rows - 1 && pos.y > 0 && pos.y < columns - 1)
                .ToList();
        }

        public bool InEvolution()
        {
            return inEvolution;
        }
    }
}