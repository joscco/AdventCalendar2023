using System;
using System.Collections.Generic;
using System.Linq;
using GameScene.Facts;
using UnityEngine;

namespace GameScene.Dialog.Area
{
    public class FactArea : MonoBehaviour
    {
        [SerializeField] private FactId areaFactId;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private bool oneTimeOnly;
        
        private HashSet<Vector2Int> _indices;

        private bool _wasEntered;

        private void Start()
        {
            spriteRenderer.color = new Color(0, 0, 0, 0);
        }

        public void OnPlayerStart(Vector2Int index)
        {
            if (HasIndex(index))
            {
                // Left
                FactManager.PublishFactAndUpdate(new Fact(areaFactId, 1));
                _wasEntered = true;
            }
            else
            {
                // Entered
                FactManager.PublishFactAndUpdate(new Fact(areaFactId, 0));
            }
        }

        public void OnPlayerMove(Vector2Int from, Vector2Int to)
        {
            if (!oneTimeOnly || !_wasEntered)
            {
                if (HasIndex(from) && HasNotIndex(to))
                {
                    // Left
                    FactManager.PublishFactAndUpdate(new Fact(areaFactId, 0));
                }
                else if (HasNotIndex(from) && HasIndex(to))
                {
                    // Entered
                    FactManager.PublishFactAndUpdate(new Fact(areaFactId, 1));
                }
            }
        }

        private bool HasNotIndex(Vector2Int index)
        {
            return !HasIndex(index);
        }

        public void SetIndices(HashSet<Vector2Int> indices)
        {
            _indices = indices;
        }

        public Bounds GetBounds()
        {
            return spriteRenderer.bounds;
        }

        private bool HasIndex(Vector2Int index)
        {
            return _indices.Contains(index);
        }
    }
}