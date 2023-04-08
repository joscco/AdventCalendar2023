using DG.Tweening;
using GameScene.Items.Item;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameScene.Items.Field
{
    public class FieldSpot : MonoBehaviour
    {
        public const float SpotWidth = 90;
        public const float SpotHeight = 75;
        
        [SerializeField] private GameObject itemSpriteHolder;
        [SerializeField] private SpriteRenderer itemSpriteRenderer;

        private bool _shown;
        private bool _itemShown;

        private ItemData _entityData;

        private void Start()
        {
            InstantBlendOut();
            InstantBlendOutItem();
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

        public void UpdateFieldEntity(ItemData fieldData, float animationDelay)
        {
            _entityData = fieldData;

            if (fieldData == null)
            {
                var sequence = DOTween.Sequence();
                sequence.AppendInterval(animationDelay);
                sequence.Append(BlendOutItem());
                sequence.AppendCallback(() => { itemSpriteRenderer.sprite = null; });
                sequence.Play();
            }
            else
            {
                if (_itemShown)
                {
                    UpdateSprite(animationDelay);
                }
                else
                {
                    itemSpriteRenderer.sprite = GetCurrentSprite(_entityData);
                    BlendInItem(animationDelay);
                }
            }
        }

        public void InstantUpdateFieldEntity(ItemData fieldData)
        {
            _entityData = fieldData;

            InstantUpdateSprite();

            if (null == fieldData)
            {
                InstantBlendOut();
            }
            else
            {
                if (!_itemShown)
                {
                    InstantBlendIn();
                }
            }
        }

        private void InstantUpdateSprite()
        {
            itemSpriteRenderer.sprite = GetCurrentSprite(_entityData);
        }

        private void UpdateSprite(float animationDelay = 0)
        {
            var sequence = DOTween.Sequence();
            sequence.AppendInterval(animationDelay);
            sequence.Append(itemSpriteHolder.transform.DOScale(0.75f, 0.1f).SetEase(Ease.InOutQuad));
            sequence.AppendCallback(() =>
            {
                itemSpriteRenderer.sprite = GetCurrentSprite(_entityData);
            });
            sequence.Append(itemSpriteHolder.transform.DOScale(1f, 0.1f).SetEase(Ease.InOutQuad));
            sequence.Play();
        }

        public ItemData GetPlantData()
        {
            return _entityData;
        }
        
        private Sprite GetCurrentSprite(ItemData entityData)
        {
            if (entityData == null)
            {
                return null;
            }
            
            return entityData.sprite;
        }
        
        public Tween BlendOut()
        {
            return transform.DOScale(0, 0.3f).SetEase(Ease.InBack);
        }

        public Tween BlendIn(float delay = 0)
        {
            return transform.DOScale(1, 0.3f).SetEase(Ease.OutBack).SetDelay(delay);
        }

        public void InstantBlendOut()
        {
            transform.localScale = Vector3.zero;
        }

        public void InstantBlendIn()
        {
            transform.localScale = Vector3.one;
        }
        
        public Tween BlendOutItem()
        {
            _itemShown = false;
            return itemSpriteHolder.transform.DOScale(0, 0.3f).SetEase(Ease.InBack);
        }

        public Tween BlendInItem(float animationDelay)
        {
            _itemShown = true;
            return itemSpriteHolder.transform.DOScale(1, 0.3f).SetEase(Ease.OutBack).SetDelay(animationDelay);
        }

        public void InstantBlendOutItem()
        {
            _itemShown = false;
            itemSpriteHolder.transform.localScale = Vector3.zero;
        }

        public void InstantBlendInItem()
        {
            _itemShown = true;
            itemSpriteHolder.transform.localScale = Vector3.one;
        }
    }
}