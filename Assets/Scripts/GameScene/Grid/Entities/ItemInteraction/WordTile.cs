using System;
using System.Collections.Generic;
using DG.Tweening;
using GameScene.Grid.Entities.ItemInteraction.Logic.Properties;
using GameScene.Grid.Entities.Shared;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace GameScene.Grid.Entities.ItemInteraction
{
    public class WordTile : MovableGridEntity
    {
        public static readonly int ItemHeight = 20;
        public static readonly int ItemJumpHeight = 40;

        public static readonly Vector2 defaultOffsetText = new Vector2(0, -50);
        public static readonly Vector2 defaultOffsetIcon = new Vector2(0, 0);

        [SerializeField] private bool interactable = true;
        [SerializeField] private InteractableItemType type;
        [SerializeField] private SpriteRenderer iconRenderer;
        [SerializeField] private TextMeshPro nameRenderer;
        [SerializeField] private TileCheckMark checkMarkRenderer;
        
        [SerializeField] private List<ParticleSystem> particleEmitters;

        private void Start()
        {
            if (null == type)
            {
                Debug.LogError("Tile Type wasn't set!");
                return;
            }
            
            iconRenderer.sprite = type.itemIcon;
            iconRenderer.transform.localPosition = defaultOffsetIcon + type.additionalOffsetImage;
            nameRenderer.transform.localPosition = defaultOffsetText + type.additionalOffsetTitle;
            UpdateName();
            LocalizationSettings.SelectedLocaleChanged += (_) => UpdateName();
        }

        private void OnValidate()
        {
            iconRenderer.sprite = type.itemIcon;
            nameRenderer.text = type.defaultName;
            iconRenderer.transform.localPosition = defaultOffsetIcon + type.additionalOffsetImage;
            nameRenderer.transform.localPosition = defaultOffsetText + type.additionalOffsetTitle;
        }
        
        private void EmitParticles()
        {
            particleEmitters.ForEach(emitter =>
            {
                emitter.Play();
            });
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
        
        public Tween RelativeMoveTo(Vector2Int newIndex, Vector3 newPos, bool emitParticlesOnEnd = false)
        {
            currentMainIndex = newIndex;
            StopMoving();
            var duration = 0.4f;
            _moveTween = DOTween.Sequence()
                .Append(transform.DOLocalMove(newPos, duration).SetEase(Ease.OutBack))
                .InsertCallback(0.15f, () =>
                {
                    if (emitParticlesOnEnd) EmitParticles();
                });
            return _moveTween;
        }
    }
}