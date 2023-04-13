using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Serialization;
using static UnityEngine.Input;

namespace GameScene.Items.Field.RectangleSelector
{
    public class PathSelector : MonoBehaviour
    {
        [SerializeField] private Camera cam;
        [FormerlySerializedAs("gridInstance")] [SerializeField] private FieldGrid grid;
        [SerializeField] private BoxCollider2D boxCollider;

        private bool _isDragging;
        private bool _isInsideField;

        private List<Vector2Int> _indexPath;

        private void Start()
        {
            boxCollider.size = grid.GetGridSizeInPixels();
            _indexPath = new List<Vector2Int>();
        }

        private void OnMouseEnter()
        {
            _isInsideField = true;
            UpdatePath(GetMousePos());
            UpdateSelectionRenderer();
        }

        private void OnMouseExit()
        {
            _isInsideField = false;
            UpdatePath(GetMousePos());
            UpdateSelectionRenderer();
        }

        private void OnMouseDown()
        {
            _isDragging = true;
            UpdatePath(GetMousePos());
            UpdateSelectionRenderer();
        }

        private void OnMouseUp()
        {
            if (_isInsideField || _isDragging)
            {
                grid.OnPathSelect(_indexPath);
            }

            _isDragging = false;
            UpdatePath(GetMousePos());
            UpdateSelectionRenderer();
        }

        private void OnMouseDrag()
        {
            UpdatePath(GetMousePos());
            UpdateSelectionRenderer();
        }

        private void OnMouseOver()
        {
            if (!_isDragging)
            {
                UpdatePath(GetMousePos());
                UpdateSelectionRenderer();
            }
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

        private void UpdatePath(Vector2 mousePosition)
        {
            var newIndex = grid.GetGridIndexForPosition(mousePosition);
            var pathLength = _indexPath.Count;

            if (_isDragging)
            {
                // Prolong or shorten path
                if (_indexPath.Count > 1 && _indexPath[pathLength - 2] == newIndex)
                {
                    // Shorten path
                    _indexPath = _indexPath.GetRange(0, pathLength - 1);
                }
                else if (AbsDiff(newIndex, _indexPath[pathLength - 1]) == 1 && !_indexPath.Contains(newIndex))
                {
                    // Prolong path
                    _indexPath.Add(newIndex);
                }
            }
            else
            {
                // Shorten down to one element
                _indexPath = new List<Vector2Int> { newIndex };
            }
        }

        private int AbsDiff(Vector2Int a, Vector2Int b)
        {
            var diff = a - b;
            return Math.Abs(diff.x) + Math.Abs(diff.y);
        }

        private void UpdateSelectionRenderer()
        {
            if (_isDragging)
            {
                grid.ShowDraggedPath(_indexPath, true);
            }
            else
            {
                if (_isInsideField && !Level.Get().inputManager.IsTouch())
                {
                    grid.ShowDraggedPath(_indexPath, false);
                }
                else
                {
                    grid.DeselectAllFields();
                }
            }
        }
    }
}