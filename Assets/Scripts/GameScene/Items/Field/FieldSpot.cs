using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using GameScene.Items.Item;
using UnityEngine;

namespace GameScene.Items.Field
{
    public class FieldSpot : MonoBehaviour
    {
        [SerializeField] private GameObject itemSpriteHolder;
        [SerializeField] private SpriteRenderer itemSpriteRenderer;
        [SerializeField] private SpriteRenderer selectionSpriteRenderer;

        private bool _shown;
        private bool _itemShown;
        private bool _itemShaking;
        private bool _selectionShown;
        private Tween _selectionFadeTween;
        private Tween _selectionScaleTween;
        private Tween _itemScaleTween;
        private Tween _shakeTween;

        private ItemData _entityData;

        private void Start()
        {
            InstantBlendOut();
            InstantBlendOutItem();
            InstantBlendOutSelection();
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
            sequence.AppendCallback(() => { itemSpriteRenderer.sprite = GetCurrentSprite(_entityData); });
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

        public void BlendInItem(float animationDelay)
        {
            _itemShown = true;
            itemSpriteHolder.transform.DOScale(1, 0.3f).SetEase(Ease.OutBack).SetDelay(animationDelay);
        }

        public void InstantBlendOutItem()
        {
            _itemShown = false;
            itemSpriteHolder.transform.localScale = Vector3.zero;
        }

        private void InstantBlendOutSelection()
        {
            _selectionShown = false;
            selectionSpriteRenderer.transform.localScale = 0.8f * Vector3.one;
            selectionSpriteRenderer.color = new Color(1, 1, 1, 0);
        }

        public void Select()
        {
            if (!_selectionShown)
            {
                _selectionShown = true;
                FadeSelectionBoxTo(1f, 0.1f);
                ScaleSelectionBoxTo(1f, 0.1f);
                ScaleItemTo(1.3f, 0.1f);
            }
        }

        public void Deselect()
        {
            if (_selectionShown)
            {
                _selectionShown = false;
                FadeSelectionBoxTo(0f, 0.1f);
                ScaleSelectionBoxTo(0.8f, 0.1f);
                ScaleItemTo(1f, 0.1f);
            }
        }

        private void ScaleItemTo(float scale, float duration)
        {
            _itemScaleTween.Kill();
            _itemScaleTween = itemSpriteRenderer
                .transform
                .DOScale(scale, duration)
                .SetEase(Ease.OutQuad);
        }

        private void FadeSelectionBoxTo(float alpha, float duration)
        {
            _selectionFadeTween.Kill();
            _selectionFadeTween = selectionSpriteRenderer
                .DOFade(alpha, duration)
                .SetEase(Ease.OutQuad);
        }

        private void ScaleSelectionBoxTo(float scale, float duration)
        {
            _selectionScaleTween.Kill();
            _selectionScaleTween = selectionSpriteRenderer
                .transform
                .DOScale(scale, duration)
                .SetEase(Ease.OutQuad);
        }

        public void BeginShakingItem()
        {
            if (!_itemShaking)
            {
                _itemShaking = true;
                _shakeTween = DOTween.Sequence()
                    .Append(itemSpriteRenderer.transform.DOLocalMoveX(3, 0.1f))
                    .Append(itemSpriteRenderer.transform.DOLocalMoveX(-3, 0.1f))
                    .SetLoops(-1);
            }
        }

        public void StopShakingItem()
        {
            if (_itemShaking)
            {
                _itemShaking = false;
                _shakeTween.Kill(true);
                itemSpriteRenderer.transform.DOLocalMoveX(0, 0.1f);
            }
        }
    }
}