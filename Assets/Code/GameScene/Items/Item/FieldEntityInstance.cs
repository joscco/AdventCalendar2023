using System;
using System.Collections;
using System.Collections.Generic;
using Code.GameScene.Inventory;
using Code.GameScene.Items.Field;
using DG.Tweening;
using UnityEngine;

namespace Code.GameScene.Items.Item
{
    public class FieldEntityInstance : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;

        private bool _shown;

        private FieldEntityData _entityData;

        private FieldSpotInstance _spot;

        private FieldEntityStatus _status;

        private int _growingLevel;

        private FieldGridInstance _gridInstance;

        public enum FieldEntityStatus
        {
            Growing,
            Finished
        }

        public void SetFieldSpot(FieldSpotInstance spot)
        {
            _spot = spot;
        }

        public InventoryItemType GetItemType()
        {
            return _entityData.itemType;
        }

        public Sprite GetInventoryIconSprite()
        {
            return _entityData.iconSprite;
        }

        private void StartEvolution()
        {
            StartCoroutine(Evolve());
        }

        private IEnumerator Evolve()
        {
            while (_growingLevel < _entityData.growingSprites.Count - 1)
            {
                yield return new WaitForSeconds(_entityData.secondsPerGrowingStage);
                _growingLevel++;
                InstantUpdateSprite();
            }

            yield return new WaitForSeconds(_entityData.secondsPerGrowingStage);
            _status = FieldEntityStatus.Finished;
            InstantUpdateSprite();
        }

        public bool CanHarvest()
        {
            return _status == FieldEntityStatus.Finished;
        }

        public Dictionary<InventoryItemType, int> Harvest()
        {
            return new Dictionary<InventoryItemType, int>()
            {
                { _entityData.itemType, 10 }
            };
        }

        public Tween BlendOut()
        {
            _shown = false;
            return transform.DOScale(0, 0.3f).SetEase(Ease.InBack);
        }

        public Tween BlendIn()
        {
            _shown = true;
            return transform.DOScale(1, 0.3f).SetEase(Ease.OutBack);
        }

        public void InstantBlendOut()
        {
            _shown = false;
            transform.localScale = Vector3.zero;
        }

        public void InstantBlendIn()
        {
            _shown = true;
            transform.localScale = Vector3.one;
        }

        public void UpdateFieldEntity(FieldEntityData fieldData)
        {
            _entityData = fieldData;
            _status = FieldEntityStatus.Growing;
            _growingLevel = 0;

            if (fieldData == null)
            {
                var sequence = DOTween.Sequence();
                sequence.Append(BlendOut());
                sequence.AppendCallback(() => { spriteRenderer.sprite = null; });
                sequence.Play();
            }
            else
            {
                if (_shown)
                {
                    var sequence = DOTween.Sequence();
                    sequence.Append(transform.DOScale(0.8f, 0.2f).SetEase(Ease.InBack));
                    sequence.AppendCallback(() =>
                    {
                        spriteRenderer.sprite = GetCurrentSprite(_entityData, _status, _growingLevel);
                    });
                    sequence.Append(transform.DOScale(1f, 0.2f).SetEase(Ease.InBack));
                    sequence.Play();
                }
                else
                {
                    spriteRenderer.sprite = GetCurrentSprite(_entityData, _status, _growingLevel);
                    BlendIn();
                }

                StartEvolution();
            }
        }

        public void InstantUpdateFieldEntity(FieldEntityData fieldData)
        {
            _entityData = fieldData;
            _status = FieldEntityStatus.Growing;
            _growingLevel = 0;

            InstantUpdateSprite();

            if (null == fieldData)
            {
                InstantBlendOut();
            }
            else
            {
                if (!_shown)
                {
                    InstantBlendIn();
                }

                StartEvolution();
            }
        }

        private void InstantUpdateSprite()
        {
            spriteRenderer.sprite = GetCurrentSprite(_entityData, _status, _growingLevel);
        }

        private Sprite GetCurrentSprite(FieldEntityData entityData, FieldEntityStatus status, int growingLevel)
        {
            if (entityData == null)
            {
                return null;
            }

            if (status == FieldEntityStatus.Finished)
            {
                return entityData.finishedSprite;
            }

            return entityData.growingSprites[growingLevel];
        }

        public void SetGrid(FieldGridInstance instance)
        {
            _gridInstance = instance;
        }
    }
}