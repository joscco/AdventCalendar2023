using DG.Tweening;
using GameScene.Items.Item;
using UnityEngine;

namespace GameScene.Items.Field
{
    public class FieldEntityInstance : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;

        private bool _shown;

        private PlantData _entityData;

        private FieldSpotInstance _spot;

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
            UpdateSprite();
        }

        public bool CanHarvest()
        {
            // Only true for plants!
            return true;
        }

        public Tween BlendOut()
        {
            Debug.Log("Blending out entity");
            _shown = false;
            return transform.DOScale(0, 0.3f).SetEase(Ease.InBack);
        }

        public Tween BlendIn()
        {
            Debug.Log("Blending in entity");
            _shown = true;
            return transform.DOScale(1, 0.3f).SetEase(Ease.OutBack);
        }

        public void InstantBlendOut()
        {
            Debug.Log("Instant Blending out entity");
            _shown = false;
            transform.localScale = Vector3.zero;
        }

        public void InstantBlendIn()
        {
            Debug.Log("Instant Blending in entity");
            _shown = true;
            transform.localScale = Vector3.one;
        }

        public void UpdateFieldEntity(PlantData fieldData)
        {
            Debug.Log("Updating with data");
            _entityData = fieldData;

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
                    Debug.Log("Blend in");
                    spriteRenderer.sprite = GetCurrentSprite(_entityData);
                    BlendIn();
                }
            }
        }

        public void InstantUpdateFieldEntity(PlantData fieldData)
        {
            _entityData = fieldData;

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
            spriteRenderer.sprite = GetCurrentSprite(_entityData);
        }

        private void UpdateSprite()
        {
            var sequence = DOTween.Sequence();
            sequence.Append(transform.DOScale(0.75f, 0.1f).SetEase(Ease.InOutQuad));
            sequence.AppendCallback(() =>
            {
                spriteRenderer.sprite = GetCurrentSprite(_entityData);
            });
            sequence.Append(transform.DOScale(1f, 0.1f).SetEase(Ease.InOutQuad));
            sequence.Play();
        }

        private Sprite GetCurrentSprite(PlantData entityData)
        {
            if (entityData == null)
            {
                return null;
            }
            
            // _status == FieldEntityStatus.THREE
            return entityData.sprite;
        }
    }
}