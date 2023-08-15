using System;
using GameScene.Facts;
using UnityEngine;

namespace GameScene.Dialog.Area
{
    public class DialogArea : MonoBehaviour
    {
        [SerializeField] private FactId areaFactId;
        [SerializeField] private int widthRight;
        [SerializeField] private int heightDown;

        public Action<Fact> onFactPublish;
        private Vector2Int _mainIndex;

        public void OnPlayerMove(Vector2Int from, Vector2Int to)
        {
            if (HasIndex(from) && HasNotIndex(to))
            {
                // Left
                onFactPublish?.Invoke(new Fact(areaFactId, 0));
                
            }
            else if (HasNotIndex(from) && HasIndex(to))
            {
                // Entered
                onFactPublish?.Invoke(new Fact(areaFactId, 1));
            }
        }

        private bool HasNotIndex(Vector2Int index)
        {
            return !HasIndex(index);
        }

        private bool HasIndex(Vector2Int index)
        {
            var diff = index - _mainIndex;
            return 0 <= diff.x
                   && diff.x <= widthRight
                   && 0 <= diff.y
                   && diff.y <= heightDown;
        }

        public void SetIndex(Vector2Int index)
        {
            _mainIndex = index;
        }
    }
}