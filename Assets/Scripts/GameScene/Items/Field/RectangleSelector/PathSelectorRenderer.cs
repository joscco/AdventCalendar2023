using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

namespace GameScene.Items.Field.RectangleSelector
{
    public class PathSelectorRenderer : MonoBehaviour
    {
        [SerializeField] private LineRenderer _lineRenderer;
        [SerializeField] private SpriteRenderer _iconRenderer;

        private static Color BLANK_WHITE = new Color(1, 1, 1, 0);

        private List<Vector2> currentPath;
        private Vector2 _currentTopRight;
        private bool _showingLine;
        private bool _showingIcon;

        private void Start()
        {
            _iconRenderer.color = BLANK_WHITE;
            _lineRenderer.startColor = BLANK_WHITE;
        }

        public void BlendOut()
        {
            BlendOutLine();
            BlendOutIcon();
        }

        private void BlendOutLine()
        {
            _lineRenderer.DOColor(new Color2(Color.black, Color.black),
                new Color2(BLANK_WHITE, BLANK_WHITE),
                0.2f).SetEase(Ease.InOutQuad);
            _showingLine = false;
        }

        private void BlendInLine()
        {
            _lineRenderer.DOColor(new Color2(BLANK_WHITE, BLANK_WHITE),
                new Color2(Color.black, Color.black),
                0.2f).SetEase(Ease.InOutQuad);
            _showingLine = true;
        }

        private void BlendInIcon()
        {
            _iconRenderer.DOFade(0.5f, 0.2f).SetEase(Ease.InOutQuad);
        }

        private void BlendOutIcon()
        {
            _iconRenderer.DOFade(0f, 0.2f).SetEase(Ease.InOutQuad);
        }

        public void ShowInactiveFieldSelection(Vector2 position)
        {
            if (_showingLine)
            {
                BlendOutLine();
            }

            if (!_showingIcon)
            {
                BlendInIcon();
            }

            UpdatePath(new List<Vector2> { position });
        }

        public void ShowEnabledPath(List<Vector2> positionPath)
        {
            if (!_showingLine)
            {
                BlendInLine();
            }

            if (!_showingIcon)
            {
                BlendInIcon();
            }

            UpdatePath(positionPath);
        }

        private void UpdatePath(List<Vector2> path)
        {
            if (currentPath == null || currentPath.Count != path.Count || currentPath[^1] != path[^1])
            {
                Vector2 lastPositionInPath = path[^1];

                _iconRenderer.transform.DOMove(lastPositionInPath, 0.1f).SetEase(Ease.InOutQuad);

                UpdateLineRenderer(path);
                currentPath = path;
            }
        }

        private void UpdateLineRenderer(List<Vector2> path)
        {
            int newLength = path.Count;
            int oldLength = currentPath != null ? currentPath.Count : 0;

            if (newLength > oldLength)
            {
                _lineRenderer.positionCount = newLength;
                for (int i = 0; i < newLength; i++)
                {
                    if (_lineRenderer.GetPosition(i) != new Vector3(path[i].x, path[i].y))
                    {
                        if (i > 0)
                        {
                            _lineRenderer.SetPosition(i, path[oldLength - 1]);
                            TweenLineVertexPosition(i, path[i]);
                        }
                        else
                        {
                            _lineRenderer.SetPosition(i, path[i]);
                        }
                    }
                }
            }
            else
            {
                _lineRenderer.positionCount = newLength;
            }
        }

        private void TweenLineVertexPosition(int i, Vector3 vector3)
        {
            DOTween.To(
                    () => _lineRenderer.GetPosition(i),
                    (val) => { _lineRenderer.SetPosition(i, val); },
                    vector3,
                    0.1f
                )
                .SetEase(Ease.InOutQuad);
        }
    }
}