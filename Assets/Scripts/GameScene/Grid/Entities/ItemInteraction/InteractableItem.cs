using System;
using DG.Tweening;
using GameScene.Facts;
using GameScene.Grid.Entities.ItemInteraction.Logic.Properties;
using GameScene.Grid.Entities.Shared;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace GameScene.Grid.Entities.ItemInteraction
{
    public class InteractableItem : MovableGridEntity
    {
        public static readonly int ItemHeight = 20;
        public static readonly int ItemJumpHeight = 40;

        [SerializeField] private bool interactable = true;
        [SerializeField] private InteractableItemType type;
        [SerializeField] private SpriteRenderer iconRenderer;
        [SerializeField] private TextMeshPro nameRenderer;

        private void Start()
        {
            if (null == type)
            {
                Debug.LogError("Tile Type wasn't set!");
                return;
            }
            
            iconRenderer.sprite = type.itemIcon;
            UpdateName();
            LocalizationSettings.SelectedLocaleChanged += (_) => UpdateName();
        }

        private void UpdateName()
        {
            nameRenderer.text = type.name.GetLocalizedString();
        }

        public void AttachToPlayer(Transform newParent, Vector3 relativePosition)
        {
            SetParent(newParent);
            // Set index to zero just in case
            MoveTo(Vector2Int.zero, relativePosition);
        }

        public InteractableItemType GetItemType()
        {
            return type;
        }

        private void SetParent(Transform newParent)
        {
            transform.parent = newParent;
        }

        public bool IsInteractable()
        {
            return interactable;
        }

        protected void SetInteractable(bool val)
        {
            interactable = val;
        }

        public virtual bool IsComplete()
        {
            return true;
        }
        
        public Tween MoveTo(Vector2Int newIndex, Vector3 newPos)
        {
            currentMainIndex = newIndex;
            return TweenRelativeMovePosition(newPos);
        }

        private Tween TweenRelativeMovePosition(Vector3 newGlobalPosition)
        {
            StopMoving();
            var duration = 0.2f;
            _moveTween = transform.DOLocalMove(newGlobalPosition, duration).SetEase(Ease.OutSine);
            return _moveTween;
        }
    }
}