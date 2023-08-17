using System;
using DG.Tweening;
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
        [SerializeField] private TileCheckMark checkMarkRenderer;

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

        private void OnValidate()
        {
            iconRenderer.sprite = type.itemIcon;
            nameRenderer.text = type.defaultName;
        }

        private void UpdateName()
        {
            nameRenderer.text = type.title.GetLocalizedString();
        }

        public InteractableItemType GetItemType()
        {
            return type;
        }

        public bool IsInteractable()
        {
            return interactable;
        }

        public void Check()
        {
            checkMarkRenderer.Show();
        }

        public void Uncheck()
        {
            checkMarkRenderer.Hide();
        }
        
        public Tween RelativeMoveTo(Vector2Int newIndex, Vector3 newPos)
        {
            currentMainIndex = newIndex;
            StopMoving();
            var duration = 0.4f;
            _moveTween = transform.DOLocalMove(newPos, duration)
                .SetEase(Ease.OutBack);
            return _moveTween;
        }
    }
}