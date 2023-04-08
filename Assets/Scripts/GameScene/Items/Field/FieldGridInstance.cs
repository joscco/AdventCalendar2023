using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using GameScene.Items.Item;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameScene.Items.Field
{
    public class FieldGridInstance : MonoBehaviour
    {
        public int columns = 20;
        public int rows = 10;

        public FieldSpotWrapperInstance spotWrapperInstance;
        public List<ItemType> randomTypes;

        private void Start()
        {
            spotWrapperInstance.SetGridInstance(this);
            BlendInField(0.5f);
            BlendInRandomItems(0.9f);
            var sequence = DOTween.Sequence();
            sequence.AppendInterval(0.5f);
            sequence.AppendCallback(() =>
            {
                Level.Get().sampleTypes.SetSampleTypes(randomTypes);
            });
            sequence.Play();
        }

        public void OnRectangleSelect(Vector2Int leftBottomIndex, Vector2Int topRightIndex)
        {
            if (Level.Get().levelData.HasActionsLeft())
            {
                var fromRow = leftBottomIndex.y;
                var toRow = topRightIndex.y;
                var fromColumn = leftBottomIndex.x;
                var toColumn = topRightIndex.x;

                for (int row = fromRow; row <= toRow; row++)
                {
                    for (int column = fromColumn; column <= toColumn; column++)
                    {
                        float delay = 0.05f * ((row - fromRow) + (column - fromColumn));
                        int curRow = row;
                        int curCol = column;
                        OnSpotSelect(curRow, curCol, delay);
                    }
                }

                Level.Get().levelData.RemoveAction();
                Level.Get().levelData.RecalculatePercentage(this);
            }
        }

        private void OnSpotSelect(int row, int column, float animationDelay)
        {
            var data = spotWrapperInstance.fieldSpotGrid[row, column].GetPlantData();
            var type = randomTypes[Random.Range(0, randomTypes.Count)];
            if (data)
            {
                var curTypeIndex = randomTypes.IndexOf(data.itemType);
                type = randomTypes[(curTypeIndex + 1) % randomTypes.Count];
            }
            
            var selectedItemData = Level.Get().itemWiki.GetItemDataForType(type);
            spotWrapperInstance.SetUpItemAt(row, column, selectedItemData, animationDelay);
        }

        private void BlendInField(float delay)
        {
            var sequence = DOTween.Sequence();
            sequence.AppendInterval(delay);
            sequence.AppendCallback(() => spotWrapperInstance.BlendInFieldGrid(rows, columns));
            sequence.Play();
        }
        
        private void BlendInRandomItems(float delay)
        {
            var sequence = DOTween.Sequence();
            sequence.AppendInterval(delay);
            sequence.AppendCallback(() =>
            {
                List<ItemData> data = randomTypes.Select(type => Level.Get().itemWiki.GetItemDataForType(type)).ToList();
                spotWrapperInstance.BlendInRandomItems(rows, columns, data);
            });
            sequence.Play();
        }

        public Vector2 GetPixelSize()
        {
            return new Vector2(
                columns * FieldSpot.SpotWidth,
                rows * FieldSpot.SpotHeight
            );
        }

        public Vector2 GetBottomLeftPoint()
        {
            return -GetPixelSize() / 2;
        }

        public RectangleSelectorGridSelection GetGridSelectionResult(
            Vector2 bottomLeftPosition,
            Vector2 topRightPosition)
        {
            Vector2 bottomLeftDiff = bottomLeftPosition - GetBottomLeftPoint();
            Vector2 topRightDiff = topRightPosition - GetBottomLeftPoint();

            var result = new RectangleSelectorGridSelection();
            int startIndexX = (int)(bottomLeftDiff.x / FieldSpot.SpotWidth);
            int startIndexY = (int)(bottomLeftDiff.y /FieldSpot.SpotHeight);
            int endIndexX = (int)(topRightDiff.x / FieldSpot.SpotWidth);
            int endIndexY = (int)(topRightDiff.y / FieldSpot.SpotHeight);

            // Sanitizing indices
            startIndexX = Math.Clamp(startIndexX, 0, columns - 1);
            startIndexY = Math.Clamp(startIndexY, 0, rows - 1);
            endIndexX = Math.Clamp(endIndexX, 0, columns - 1);
            endIndexY = Math.Clamp(endIndexY, 0, rows - 1);

            Vector2 leftBottom = GetBottomLeftPoint()
                                 + startIndexX * Vector2.right * FieldSpot.SpotWidth
                                 + startIndexY * Vector2.up * FieldSpot.SpotHeight;
            Vector2 topRight = GetBottomLeftPoint()
                               + (endIndexX + 1) * Vector2.right * FieldSpot.SpotWidth
                               + (endIndexY + 1) * Vector2.up * FieldSpot.SpotHeight;

            result.BottomLeftIndex = new Vector2Int(startIndexX, startIndexY);
            result.TopRightIndex = new Vector2Int(endIndexX, endIndexY);
            result.BottomLeftPosition = leftBottom;
            result.TopRightPosition = topRight;
            
            return result;
        }

        public class RectangleSelectorGridSelection
        {
            public Vector2Int BottomLeftIndex;
            public Vector2Int TopRightIndex;
            public Vector2 BottomLeftPosition;
            public Vector2 TopRightPosition;
        }
    }
}