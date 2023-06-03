using System;
using System.Collections.Generic;
using GameScene.Items.Field;
using UnityEngine;
using static UnityEngine.Input;

namespace GameScene.PathSelector
{
    public class PathSelector : MonoBehaviour
    {
        [SerializeField] private Camera cam;
        [SerializeField] private FieldGrid grid;
        [SerializeField] private BoxCollider2D boxCollider;
        [SerializeField] private PathSelectorRenderer pathRenderer;

        private bool _isDragging;
        private bool _isInsideField;

        private List<Vector2Int> _draggedIndexPath;
        private Vector2Int _hoveredIndex;

        private void Start()
        {
            boxCollider.size = grid.GetGridSizeInPixels();
            _draggedIndexPath = new List<Vector2Int>();
            _hoveredIndex = new Vector2Int();
        }

        private void OnMouseEnter()
        {
            _isInsideField = true;

            if (!_isDragging)
            {
                HoverUpdateField(GetMousePos());
            }
        }
        
        private void OnMouseOver()
        {
            if (!_isDragging)
            {
                HoverUpdateField(GetMousePos());
            }
        }

        private void OnMouseExit()
        {
            _isInsideField = false;
            
            if (!_isDragging)
            {
                BlendOutHoveredField();
            }
        }

        private void OnMouseDown()
        {
            _isDragging = true;

            PreDragUpdate(GetMousePos());
        }

        private void OnMouseUp()
        {
            if (_isInsideField || _isDragging)
            {
                grid.OnPathSelect(_draggedIndexPath);
            }

            _isDragging = false;

            PostDragUpdatePath(GetMousePos());
        }

        private void OnMouseDrag()
        {
            DragUpdatePath(GetMousePos());
        }

        // Handlers
        private void PreDragUpdate(Vector2 currentMousePos)
        {
            ResetHoveredField();
            ResetDraggedPath();
            
            var firstIndex = grid.GetGridIndexForPosition(currentMousePos);
            _draggedIndexPath = new List<Vector2Int> { firstIndex };
            pathRenderer.ShowDraggedPath(_draggedIndexPath);
        }
        
        private void DragUpdatePath(Vector2 currentMousePos)
        {
            var currentIndex = grid.GetGridIndexForPosition(currentMousePos);
            var pathLength = _draggedIndexPath.Count;
            var lastIndex = _draggedIndexPath[^1];
            
            // Shorten path if element before last was index
            if (_draggedIndexPath.Count > 1 && _draggedIndexPath[pathLength - 2] == currentIndex)
            {
                _draggedIndexPath.RemoveAt(pathLength - 1);
                pathRenderer.ShowDraggedPath(_draggedIndexPath);
            }
            // Or make it longer if it's not contained and valid
            else if (!_draggedIndexPath.Contains(currentIndex) && AreNeighbors(lastIndex, currentIndex))
            {
                _draggedIndexPath.Add(currentIndex);
                pathRenderer.ShowDraggedPath(_draggedIndexPath);
            }
        }
        
        private void HoverUpdateField(Vector2 currentMousePos)
        {
            var currentIndex = grid.GetGridIndexForPosition(currentMousePos);
            if (_hoveredIndex != currentIndex)
            {
                _hoveredIndex = currentIndex;
                pathRenderer.ShowHoveredField(_hoveredIndex);
            }
        }
        
        private void BlendOutHoveredField()
        {
            ResetHoveredField();
            pathRenderer.BlendOutHoveredField();
        }

        private void PostDragUpdatePath(Vector2 currentMousePos)
        {
            ResetDraggedPath();
            
            if (_isInsideField)
            {
                var currentIndex = grid.GetGridIndexForPosition(currentMousePos);
                _hoveredIndex = currentIndex;
                pathRenderer.ShowHoveredField(_hoveredIndex);
            }
            else
            {
                pathRenderer.BlendOutDraggedPath();
            }
        }
        
        // Helpers
        private void ResetDraggedPath()
        {
            _draggedIndexPath = new List<Vector2Int>();
        }
        
        private void ResetHoveredField()
        {
            _hoveredIndex = new Vector2Int(-1, -1);
        }
        private Vector2 GetMousePos()
        {
            if (cam != null)
            {
                return cam.ScreenToWorldPoint(mousePosition);
            }

            Debug.Log("Camera not defined!");
            return Vector2.zero;
        }
        
        private int AbsDiff(Vector2Int a, Vector2Int b)
        {
            var diff = a - b;
            return Math.Abs(diff.x) + Math.Abs(diff.y);
        }

        private bool AreNeighbors(Vector2Int a, Vector2Int b)
        {
            return AbsDiff(a, b) == 1;
        }
        
        private bool IsNotTouchDisplay()
        {
            return !Level.Get().inputManager.IsTouch();
        }
    }
}