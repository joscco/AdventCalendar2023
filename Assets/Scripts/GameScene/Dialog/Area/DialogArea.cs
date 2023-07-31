using System;
using GameScene.Dialog.Background;
using GameScene.Dialog.Data;
using UnityEngine;

namespace GameScene.Dialog.Area
{
    public class DialogArea : MonoBehaviour
    {
        [SerializeField] private DialogFactId areaFactId;
        [SerializeField] private int widthRight;
        [SerializeField] private int heightDown;

        private DialogManager _dialogManager;
        private Vector2Int _mainIndex;

        private void Start()
        {
            _dialogManager = FindObjectOfType<DialogManager>();
        }

        public void OnPlayerMove(Vector2Int from, Vector2Int to)
        {
            if (HasIndex(from) && HasNotIndex(to))
            {
                // Left
                _dialogManager.PublishFactAndUpdate(new DialogFact(areaFactId, 0));
            }
            else if (HasNotIndex(from) && HasIndex(to))
            {
                // Entered
                _dialogManager.PublishFactAndUpdate(new DialogFact(areaFactId, 1));
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