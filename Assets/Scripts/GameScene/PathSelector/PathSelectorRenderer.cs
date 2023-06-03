using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using GameScene.Items.Field;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameScene.PathSelector
{
    public class PathSelectorRenderer : MonoBehaviour
    {
        [SerializeField] private FieldGrid grid;
        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] private SpriteRenderer hoverFieldRenderer;

        private static Color BLANK_WHITE = new Color(1, 1, 1, 0);

        private List<Vector2> currentPath;
        private bool _showingLine;
        private bool _showingHoverField;
        private Sequence _updateLineSequence;

        private void Start()
        {
            lineRenderer.startColor = BLANK_WHITE;
            hoverFieldRenderer.color = BLANK_WHITE;
            currentPath = new List<Vector2>();
        }

        public void ShowHoveredField(Vector2Int hoveredIndex)
        {
            if (_showingLine)
            {
                _showingLine = false;
                currentPath = new List<Vector2>();
                BlendOutLine();
            }

            Vector3 newFieldPosition = grid.GetPositionForGridIndex(hoveredIndex);
            if (!_showingHoverField)
            {
                BlendInHoveredField(newFieldPosition);
            }
            else
            {
                MoveHoveredField(newFieldPosition);
            }
        }

        public void ShowDraggedPath(List<Vector2Int> positionPath)
        {
            if (_showingHoverField)
            {
                _showingHoverField = false;
                BlendOutHoveredField();
            }

            if (!_showingLine)
            {
                _showingLine = true;
                BlendInLine();
            }

            List<Vector2> rawPositionPath = positionPath
                .Select(i => grid.GetPositionForGridIndex(i))
                .ToList();
            List<Vector2> roundedPositionPath = RoundPositionPath(rawPositionPath);
            UpdateLine(roundedPositionPath);
        }

        private List<Vector2> RoundPositionPath(List<Vector2> rawPositionPath)
        {
            List<Vector2> result = new List<Vector2>();

            for (int i = 0; i < rawPositionPath.Count - 1; i++)
            {
                Vector2 a = rawPositionPath[i];
                Vector2 b = rawPositionPath[i + 1];

                result.Add(a);
                result.Add(a + 0.05f * (b - a));
                result.Add(a + 0.95f * (b - a));
            }

            result.Add(rawPositionPath[^1]);
            return result;
        }

        public void BlendOutDraggedPath()
        {
            if (_showingLine)
            {
                _showingLine = false;
                BlendOutLine();
            }
        }

        public void BlendInHoveredField(Vector3 pos)
        {
            if (!_showingHoverField)
            {
                _showingHoverField = true;
                hoverFieldRenderer.transform.position = pos;
                BlendInField();
            }
        }

        public void BlendOutHoveredField()
        {
            if (_showingHoverField)
            {
                _showingHoverField = false;
                BlendOutField();
            }
        }

        // Private stuff

        private void MoveHoveredField(Vector3 pos)
        {
            hoverFieldRenderer.transform.DOMove(pos, 0.2f)
                .SetEase(Ease.InOutQuad);
        }

        private void BlendOutLine()
        {
            lineRenderer.DOColor(new Color2(Color.black, Color.black),
                new Color2(BLANK_WHITE, BLANK_WHITE),
                0.2f).SetEase(Ease.InOutQuad);
        }

        private void BlendInLine()
        {
            lineRenderer.DOColor(new Color2(BLANK_WHITE, BLANK_WHITE),
                new Color2(Color.black, Color.black),
                0.2f).SetEase(Ease.InOutQuad);
        }

        private void BlendInField()
        {
            hoverFieldRenderer.DOFade(1f, 0.3f).SetEase(Ease.InOutQuad);
        }

        private void BlendOutField()
        {
            hoverFieldRenderer.DOFade(0f, 0.3f).SetEase(Ease.InOutQuad);
        }

        private void UpdateLine(List<Vector2> path)
        {
            if (!currentPath.SequenceEqual(path))
            {
                UpdateLineRenderer(currentPath, path);
                currentPath = path;
            }
        }

        private void UpdateLineRenderer(List<Vector2> oldPath, List<Vector2> newPath)
        {
            int commonRootLength = FindCommonLength(oldPath, newPath);
            int lastOldPathIndex = oldPath.Count - 1;

            // Do instant reset
            _updateLineSequence?.Kill();
            lineRenderer.positionCount = oldPath.Count;
            lineRenderer.SetPositions(oldPath.Select(x => new Vector3(x.x, x.y, 0)).ToArray());

            _updateLineSequence = DOTween.Sequence();

            if (oldPath.Count > 0)
            {
                // Shorten Line to common Length
                for (int i = Math.Max(1, lastOldPathIndex); i > commonRootLength; i--)
                {
                    var index = i;
                    _updateLineSequence.InsertCallback((lastOldPathIndex - i) * 0.05f, () =>
                    {
                        TweenLineVertexPosition(index, oldPath[index - 1]);
                        lineRenderer.positionCount = index;
                    });
                }
            }

            // Blend back line
            for (int i = commonRootLength; i < newPath.Count; i++)
            {
                var index = i;
                var prevPos = i > 0 ? newPath[i - 1] : newPath[0];
                var pos = newPath[i];
                _updateLineSequence.InsertCallback(i * 0.05f, () =>
                {
                    lineRenderer.positionCount = index + 1;
                    lineRenderer.SetPosition(index, prevPos);
                    TweenLineVertexPosition(index, pos);
                });
            }

            _updateLineSequence.Play();
        }

        private int FindCommonLength(List<Vector2> oldPath, List<Vector2> newPath)
        {
            int commonRootLength = 0;
            for (int i = 0; i < Math.Min(oldPath.Count, newPath.Count); i++)
            {
                if (oldPath[i] != newPath[i])
                {
                    break;
                }

                commonRootLength++;
            }

            return commonRootLength;
        }

        private Tween TweenLineVertexPosition(int i, Vector3 aim)
        {
            Debug.Log("LineRendererCount: " + lineRenderer.positionCount);
            Debug.Log("Index: " + i);
            return DOTween.To(
                    () => lineRenderer.GetPosition(i),
                    (val) => { lineRenderer.SetPosition(i, val); },
                    aim,
                    0.2f
                )
                .SetEase(Ease.InOutQuad);
        }
    }
}