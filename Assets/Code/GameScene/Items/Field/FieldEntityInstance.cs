using System.Collections.Generic;
using Code.GameScene.Inventory;
using Code.GameScene.Items.Item;
using DG.Tweening;
using UnityEngine;

namespace Code.GameScene.Items.Field
{
    public class FieldEntityInstance : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;

        private bool _shown;

        private PlantData _entityData;

        private FieldSpotInstance _spot;

        private FieldEntityStatus _status;

        public enum FieldEntityStatus
        {
            ONE = 1,
            TWO = 2,
            THREE = 3
        }

        public PlantType GetItemType()
        {
            return _entityData.plantType;
        }

        public PlantData GetPlantData()
        {
            return _entityData;
        }

        public Sprite GetInventoryIconSprite()
        {
            return _entityData.inventoryIconSprite;
        }

        public void Evolve()
        {
            switch (_status)
            {
                case FieldEntityStatus.ONE:
                    _status = FieldEntityStatus.TWO;
                    break;
                case FieldEntityStatus.TWO:
                    _status = FieldEntityStatus.THREE;
                    break;
                case FieldEntityStatus.THREE:
                    _status = FieldEntityStatus.ONE;
                    UpdateFieldEntity(null);
                    break;
            }
            UpdateSprite();
        }

        public bool CanHarvest()
        {
            // Only true for plants!
            return true;
        }

        public Dictionary<PlantType, int> Harvest()
        {
            return new Dictionary<PlantType, int>()
            {
                { _entityData.plantType, (int) _status}
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

        public void UpdateFieldEntity(PlantData fieldData)
        {
            _entityData = fieldData;
            _status = FieldEntityStatus.ONE;

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
                    UpdateSprite();
                }
                else
                {
                    spriteRenderer.sprite = GetCurrentSprite(_entityData, _status);
                    BlendIn();
                }
            }
        }

        public void InstantUpdateFieldEntity(PlantData fieldData)
        {
            _entityData = fieldData;
            _status = FieldEntityStatus.ONE;

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
            }
        }

        private void InstantUpdateSprite()
        {
            spriteRenderer.sprite = GetCurrentSprite(_entityData, _status);
        }

        private void UpdateSprite()
        {
            var sequence = DOTween.Sequence();
            sequence.Append(transform.DOScale(0.75f, 0.1f).SetEase(Ease.InOutQuad));
            sequence.AppendCallback(() =>
            {
                spriteRenderer.sprite = GetCurrentSprite(_entityData, _status);
            });
            sequence.Append(transform.DOScale(1f, 0.1f).SetEase(Ease.InOutQuad));
            sequence.Play();
        }

        private Sprite GetCurrentSprite(PlantData entityData, FieldEntityStatus status)
        {
            if (entityData == null)
            {
                return null;
            }

            if (status == FieldEntityStatus.ONE)
            {
                return entityData.oneSprite;
            }
            
            if (status == FieldEntityStatus.TWO)
            {
                return entityData.twoSprite;
            }
            
            // _status == FieldEntityStatus.THREE
            return entityData.threeSprite;
        }
    }
}