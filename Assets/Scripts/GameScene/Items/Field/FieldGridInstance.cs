using System.Collections.Generic;
using DG.Tweening;
using GameScene.Items.Item;
using UnityEngine;

namespace GameScene.Items.Field
{
    public class FieldGridInstance : MonoBehaviour
    {
        public int Columns = 20;
        public int Rows = 10;

        public FieldSpotWrapperInstance spotWrapperInstance;
        public List<PlantType> randomTypes;

        private void Start()
        {
            spotWrapperInstance.SetGridInstance(this);
            BlendInField();
        }

        private void SubtractAction()
        {
            Level.Get().levelData.RemoveAction();
        }

        public void OnRectangleSelect(int fromRow, int toRow, int fromColumn, int toColumn)
        {
            for (int row = fromRow; row <= toRow; row++)
            {
                for (int column = fromColumn; column <= toColumn; column++)
                {
                    OnSpotSelect(row, column);
                }
            }

            SubtractAction();
        }

        private void OnSpotSelect(int row, int column)
        {
            if (Level.Get().levelData.HasActionsLeft() && !spotWrapperInstance.InEvolution())
            {
                var type = randomTypes[Random.Range(0, randomTypes.Count)];
                var selectedItemData = Level.Get().plantWiki.GetPlantDataForPlant(type);
                spotWrapperInstance.SetUpItemAt(row, column, selectedItemData);
            }
        }

        private void BlendInField()
        {
            var sequence = DOTween.Sequence();
            sequence.AppendInterval(0.5f);
            sequence.AppendCallback(() => spotWrapperInstance.BlendInFieldGrid(Rows, Columns));
            sequence.Play();
        }

        public Vector2 GetPixelSize()
        {
            return new Vector2(
                Columns * FieldSpotWrapperInstance.SpotWidth,
                Rows * FieldSpotWrapperInstance.SpotHeight
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
            
            // Für dimensionen <= 2 scheint dies ein Problem zu sein -> Debuggen
            Vector2 bottomLeftDiff = bottomLeftPosition - GetBottomLeftPoint();
            Vector2 topRightDiff = topRightPosition - GetBottomLeftPoint();

            var result = new RectangleSelectorGridSelection();
            int startIndexX = (int)(bottomLeftDiff.x / FieldSpotWrapperInstance.SpotWidth);
            int startIndexY = (int)(bottomLeftDiff.y / FieldSpotWrapperInstance.SpotHeight);
            int endIndexX = (int)(topRightDiff.x / FieldSpotWrapperInstance.SpotWidth);
            int endIndexY = (int)(topRightDiff.y / FieldSpotWrapperInstance.SpotHeight);

            Vector2 leftBottom = GetBottomLeftPoint()
                                 + startIndexX * Vector2.right * FieldSpotWrapperInstance.SpotWidth
                                 + startIndexY * Vector2.up * FieldSpotWrapperInstance.SpotHeight;
            Vector2 topRight = GetBottomLeftPoint()
                               + (endIndexX + 1) * Vector2.right * FieldSpotWrapperInstance.SpotWidth
                               + (endIndexY + 1) * Vector2.up * FieldSpotWrapperInstance.SpotHeight;

            result.bottomLeftIndex = new Vector2Int(startIndexX, startIndexY);
            result.topRightIndex = new Vector2Int(endIndexX, endIndexY);
            result.leftBottomPosition = leftBottom;
            result.rightTopPosition = topRight;
            return result;
        }

        public class RectangleSelectorGridSelection
        {
            public Vector2Int bottomLeftIndex;
            public Vector2Int topRightIndex;
            public Vector2 leftBottomPosition;
            public Vector2 rightTopPosition;
        }
    }
}