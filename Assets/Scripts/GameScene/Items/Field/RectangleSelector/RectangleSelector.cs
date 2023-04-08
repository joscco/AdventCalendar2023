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

        private bool _isDragging;
        private bool _isInsideField;

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
            _isInsideField = true;
            UpdateRawBox(GetMousePos());
            UpdateSelectionRenderer();
        }

        private void OnMouseExit()
        {
            _isInsideField = false;
            UpdateRawBox(GetMousePos());
            UpdateSelectionRenderer();
        }

        private void OnMouseDown()
        {
            _isDragging = true;
            _startSelectionMousePos = GetMousePos();
            UpdateRawBox(GetMousePos());
            UpdateSelectionRenderer();
        }

        private void OnMouseUp()
        {
            if ((_isInsideField || _isDragging) && _gridResult != null)
            {
                Vector2Int bottomLeft = _gridResult.BottomLeftIndex;
                Vector2Int topRight = _gridResult.TopRightIndex;

                gridInstance.OnRectangleSelect(bottomLeft, topRight);
            }

            _isDragging = false;
            UpdateRawBox(GetMousePos());
            UpdateSelectionRenderer();
        }

        private void OnMouseDrag()
        {
            UpdateRawBox(GetMousePos());
            UpdateSelectionRenderer();
        }

        private void OnMouseOver()
        {
            UpdateRawBox(GetMousePos());
            UpdateSelectionRenderer();
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

        private void UpdateRawBox(Vector2 currentMousePos)
        {
            if (_isDragging)
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

            _gridResult = gridInstance.GetGridSelectionResult(
                _rawDownLeftOfSelectionBox,
                _rawTopRightSelectionBox
            );
        }

        private void UpdateSelectionRenderer()
        {
            if (selectionBoxRenderer != null)
            {
                if (_isDragging)
                {
                    selectionBoxRenderer.ShowActiveEnabledAt(
                        _gridResult.BottomLeftPosition,
                        _gridResult.TopRightPosition
                    );
                }
                else
                {
                    if (_isInsideField)
                    {
                        selectionBoxRenderer.ShowInactiveAt(
                            _gridResult.BottomLeftPosition,
                            _gridResult.TopRightPosition
                        );
                    }
                    else
                    {
                        selectionBoxRenderer.BlendOut();
                    }
                }
            }
        }
    }
}