using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using GameScene.Items.Item;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameScene.Items.Field
{
    public class FieldGrid : MonoBehaviour
    {
        public float SpotWidth = 80;
        public float SpotHeight = 80;
        public int Columns = 20;
        public int Rows = 7;

        public List<ItemType> randomTypes;
        public FieldSpot spotPrefab;

        private FieldSpot[] fieldSpots;
        private FieldSpot[,] fieldSpotGrid;
        private List<Vector2Int> _shownPath;
        private bool _draggingPath;

        private void Start()
        {
            InitFieldSpots();
            InitFieldGrid(fieldSpots);

            var sequence = DOTween.Sequence();
            sequence.AppendInterval(0.5f);
            sequence.AppendCallback(() =>
            {
                BlendInFieldGrid();
                BlendInRandomItemsOnField();
                Level.Get().sampleTypes.SetSampleTypes(randomTypes);
            });
            sequence.Play();
        }

        private float CalculatePercentage()
        {
            return fieldSpots
                .Where(spot => spot.GetPlantData())
                .Select(data => data.GetPlantData().itemType)
                .Count(type => type == ItemType.Roses) * 1f / fieldSpots.Length;
        }

        private void InitFieldSpots()
        {
            int numberOfRenderers = Rows * Columns;
            fieldSpots = new FieldSpot[numberOfRenderers];

            for (int i = 0; i < numberOfRenderers; i++)
            {
                fieldSpots[i] = Instantiate(spotPrefab, transform);
            }
        }

        private void InitFieldGrid(FieldSpot[] spots)
        {
            fieldSpotGrid = new FieldSpot[Rows, Columns];
            for (int row = 0; row < Rows; row++)
            {
                for (int column = 0; column < Columns; column++)
                {
                    var fieldSpot = spots[Columns * row + column];
                    var pos = GetSpotPosition(row, Rows, column, Columns);
                    fieldSpot.transform.position = pos;
                    fieldSpot.InstantUpdateFieldEntity(null);

                    fieldSpotGrid[row, column] = fieldSpot;
                }
            }
        }

        public void BlendInFieldGrid()
        {
            InstantBlendOutAllSpots();

            var sequence = DOTween.Sequence();
            for (int row = 0; row < Rows; row++)
            {
                for (int column = 0; column < Columns; column++)
                {
                    sequence.Insert(0, fieldSpotGrid[row, column]
                        .BlendIn(0.5f + (row + column) * 0.05f));
                }
            }

            sequence.InsertCallback(
                0.5f + (Rows + Columns) * 0.05f,
                () => Level.Get().levelData.UpdatePercentage(CalculatePercentage())
            );

            sequence.Play();
        }

        public void BlendInRandomItemsOnField(int rows, int columns, List<ItemData> randomData)
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
            foreach (var fieldSpotRenderer in fieldSpots)
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

        public void SetUpItemAt(int row, int column, ItemData selectedItemData, float animationDelay)
        {
            fieldSpotGrid[row, column].UpdateFieldEntity(selectedItemData, animationDelay);
        }

        public void SelectField(Vector2Int index)
        {
            fieldSpotGrid[index.y, index.x].Select();
        }

        public void SelectPath(List<Vector2Int> indexPath, bool dragging)
        {
            foreach (var index in indexPath)
            {
                fieldSpotGrid[index.y, index.x].Select();
                
                if (dragging)
                {
                    fieldSpotGrid[index.y, index.x].BeginShakingItem();
                }
            }
        }

        private void DeselectAndStopShaking(Vector2Int index)
        {
            fieldSpotGrid[index.y, index.x].Deselect();
            fieldSpotGrid[index.y, index.x].StopShakingItem();
        }
        
        private void StopShaking(Vector2Int index)
        {
            fieldSpotGrid[index.y, index.x].StopShakingItem();
        }

        public void OnPathSelect(List<Vector2Int> path)
        {
            if (Level.Get().levelData.HasActionsLeft())
            {
                for (int i = 0; i < path.Count; i++)
                {
                    var index = path[i];
                    float delay = 0.01f * i;
                    OnSpotSelect(index.y, index.x, delay);
                }

                Level.Get().levelData.RemoveAction();
                Level.Get().levelData.UpdatePercentage(CalculatePercentage());
            }
        }

        private void OnSpotSelect(int row, int column, float animationDelay)
        {
            var data = fieldSpotGrid[row, column].GetPlantData();
            var type = randomTypes[Random.Range(0, randomTypes.Count)];
            if (data)
            {
                var curTypeIndex = randomTypes.IndexOf(data.itemType);
                type = randomTypes[(curTypeIndex + 1) % randomTypes.Count];
            }

            var selectedItemData = Level.Get().itemWiki.GetItemDataForType(type);
            SetUpItemAt(row, column, selectedItemData, animationDelay);
        }

        private void BlendInRandomItemsOnField()
        {
            List<ItemData> data = randomTypes
                .Select(type => Level.Get().itemWiki.GetItemDataForType(type))
                .ToList();
            BlendInRandomItemsOnField(Rows, Columns, data);
        }

        public Vector2 GetGridSizeInPixels()
        {
            return new Vector2(
                Columns * SpotWidth,
                Rows * SpotHeight
            );
        }

        public Vector2 GetBottomLeftGridPoint()
        {
            return -GetGridSizeInPixels() / 2;
        }

        public Vector2Int GetGridIndexForPosition(Vector2 mousePosition)
        {
            Vector2 diff = mousePosition - GetBottomLeftGridPoint();

            int indexX = (int)(diff.x / SpotWidth);
            int indexY = (int)(diff.y / SpotHeight);

            // Sanitizing indices
            indexX = Math.Clamp(indexX, 0, Columns - 1);
            indexY = Math.Clamp(indexY, 0, Rows - 1);

            return new Vector2Int(indexX, indexY);
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

        public void ShowDraggedPath(List<Vector2Int> newPath, bool dragging)
        {
            if (_shownPath == null 
                || _shownPath.Count != newPath.Count 
                || _shownPath[^1] != newPath[^1] 
                || dragging != _draggingPath)
            {
                DeselectAllFields(newPath, dragging);
                SelectPath(newPath, dragging);
                // Use a copy here!
                _shownPath = new List<Vector2Int>(newPath);
                _draggingPath = dragging;
            }
        }

        public void DeselectAllFields(List<Vector2Int> newPath = null, bool dragging = false)
        {
            if (_shownPath != null)
            {
                if (newPath != null)
                {
                    List<Vector2Int> incommonFields = _shownPath
                        .Where(index => !newPath.Contains(index))
                        .ToList();
                    List<Vector2Int> commonFields = _shownPath
                        .Where(index => newPath.Contains(index))
                        .ToList();
                    
                    foreach (var index in incommonFields)
                    {
                        DeselectAndStopShaking(index);
                    }

                    if (!dragging && _draggingPath)
                    {
                        foreach (var index in commonFields)
                        {
                            StopShaking(index);
                        }
                    }
                }
                else
                {
                    foreach (var index in _shownPath)
                    {
                        DeselectAndStopShaking(index);
                    }
                }

                _shownPath = newPath;
            }
        }
    }
}