using System;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Input;

namespace GameScene.Items.Field.RectangleSelector
{
    public class RectangleSelector : MonoBehaviour
    {
        [SerializeField] private Camera cam;
        [SerializeField] private FieldGridInstance gridInstance;
        [SerializeField] private BoxCollider2D boxCollider;
        [SerializeField] private RectangleSelectorRenderer selectionBoxRenderer;

        private bool _dragging;
        private bool _showingSelection;

        private Vector2 _startSelectionMousePos;

        private Vector2 _rawDownLeftOfSelectionBox;
        private Vector2 _rawTopRightSelectionBox;

        private Vector2 _downLeftOfSelectionBox;
        private Vector2 _topRightSelectionBox;

        private FieldGridInstance.RectangleSelectorGridSelection _gridResult;

        private void Start()
        {
            boxCollider.size = gridInstance.GetPixelSize();
        }

        private void OnMouseEnter()
        {
            if (!_dragging)
            {
                _showingSelection = true;
                UpdateSelectionRenderer();
            }
        }

        private void OnMouseExit()
        {
            if (!_dragging)
            {
                _showingSelection = false;
                UpdateSelectionRenderer();
            }
        }

        private void OnMouseDown()
        {
            _dragging = true;
            _startSelectionMousePos = GetMousePos();
        }

        private Vector2 GetMousePos()
        {
            if (cam != null)
            {
                return cam.ScreenToWorldPoint(mousePosition);
            }
            else
            {
                Debug.Log("Camera not defined!");
            }

            return Vector2.zero;
        }

        private void OnMouseUp()
        {
            UpdateRawBox(GetMousePos());
            UpdateSelectionRenderer();

            if (_showingSelection && _gridResult != null)
            {
                Vector2Int bottomLeft = _gridResult.bottomLeftIndex;
                Vector2Int topRight = _gridResult.topRightIndex;
                gridInstance.OnRectangleSelect(
                    bottomLeft.y,
                    topRight.y,
                    bottomLeft.x,
                    topRight.x
                );
            }

            _rawDownLeftOfSelectionBox = GetMousePos();
            _rawTopRightSelectionBox = GetMousePos();
            _dragging = false;
        }

        private void OnMouseOver()
        {
            UpdateRawBox(GetMousePos());
            UpdateSelectionRenderer();
        }

        private void UpdateRawBox(Vector2 currentMousePos)
        {
            if (_dragging)
            {
                float minX = Math.Min(currentMousePos.x, _startSelectionMousePos.x);
                float minY = Math.Min(currentMousePos.y, _startSelectionMousePos.y);
                float maxX = Math.Max(currentMousePos.x, _startSelectionMousePos.x);
                float maxY = Math.Max(currentMousePos.y, _startSelectionMousePos.y);

                _rawDownLeftOfSelectionBox = new Vector2(minX, minY);
                _rawTopRightSelectionBox = new Vector2(maxX, maxY);
            }
            else
            {
                _rawDownLeftOfSelectionBox = currentMousePos;
                _rawTopRightSelectionBox = currentMousePos;
            }
        }

        private void UpdateSelectionRenderer()
        {
            _gridResult = gridInstance.GetGridSelectionResult(_rawDownLeftOfSelectionBox, _rawTopRightSelectionBox);
            if (selectionBoxRenderer != null)
            {
                if (_showingSelection)
                {
                    if (_dragging)
                    {
                        selectionBoxRenderer.ShowActiveEnabledAt(
                            _gridResult.leftBottomPosition,
                            _gridResult.rightTopPosition
                        );
                    }
                    else
                    {
                        selectionBoxRenderer.ShowInactiveAt(
                            _gridResult.leftBottomPosition,
                            _gridResult.rightTopPosition
                        );
                    }
                }
                else
                {
                    selectionBoxRenderer.BlendOut();
                }
            }
        }
    }
}