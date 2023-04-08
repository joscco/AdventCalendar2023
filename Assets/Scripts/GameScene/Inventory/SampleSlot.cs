using DG.Tweening;
using GameScene.Items.Item;
using TMPro;
using UnityEngine;

namespace GameScene.Inventory.Renderer
{
    public class SampleSlot : MonoBehaviour
    {
        private ItemData _slotData;
        
        [SerializeField] private SpriteRenderer itemSpriteRenderer;

        private SampleList _sampleList;

        private bool _shown;
        private bool _upSized;

        private void Start()
        {
            transform.localScale = Vector3.zero;
        }

        public void OnMouseEnter()
        {
            if (!_upSized)
            {
                transform.DOScale(1.1f, 0.3f).SetEase(Ease.OutBack);
            }
        }

        public void OnMouseExit()
        {
            if (!_upSized)
            {
                transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack);
            }
        }

        public void OnMouseUp()
        {
            _sampleList.SelectItemSlot(this);
        }

        public Tween UpdateSlot(ItemData newSlotInfo)
        {
            if (null == newSlotInfo)
            {
                return BlendOutThenErase();
            }
            else
            {
                var oldSlotInfo = _slotData;
                _slotData = newSlotInfo;
                
                if (_shown)
                {
                    if (oldSlotInfo == null || newSlotInfo.itemType != oldSlotInfo.itemType)
                    {
                        return WiggleAndUpdate(newSlotInfo.itemType);
                    }

                    return null;
                }

                return UpdateThenBlendIn(newSlotInfo.itemType);
            }
        }

        public void InstantUpdateSlot(ItemData newSlotInfo)
        {
            if (null == newSlotInfo)
            {
                transform.localScale = Vector3.zero;
                EraseData();
            }
            else
            {
                transform.localScale = Vector3.one;
                ChangeItemSprite(newSlotInfo.itemType);
            }
        }

        public ItemType GetItemType()
        {
            return _slotData.itemType;
        }

        private Tween UpdateThenBlendIn(ItemType type)
        {
            ChangeItemSprite(type);
            return BlendIn();
        }

        private Tween BlendOutThenErase()
        {
            var sequence = DOTween.Sequence();
            sequence.Append(BlendOut());
            sequence.AppendCallback(() => EraseData());
            sequence.Play();

            return sequence;
        }

        private Tween WiggleAndUpdate(ItemType type)
        {
            float preScale = _sampleList.GetSelectedSlot() == this ? 0.85f : 0.75f;
            float postScale = _sampleList.GetSelectedSlot() == this ? 1.2f : 1f;
            
            var sequence = DOTween.Sequence();
            sequence.Append(transform.DOScale(preScale, 0.2f)).SetEase(Ease.OutBack);
            sequence.Append(transform.DOScale(postScale, 0.2f)).SetEase(Ease.OutBack);
            sequence.AppendCallback(() =>
            {
                ChangeItemSprite(type);
            });
            sequence.Play();

            return sequence;
        }

        private void ChangeItemSprite(ItemType newItemType)
        {
            itemSpriteRenderer.sprite = Level.Get().itemWiki.GetSamplesIconSpriteForType(newItemType);
        }

        private Tween BlendIn()
        {
            _shown = true;
            return transform.DOScale(1, 0.5f).SetEase(Ease.OutBack);
        }

        private Tween BlendOut()
        {
            _shown = false;
            return transform.DOScale(0, 0.5f).SetEase(Ease.InBack);
        }

        private void EraseData()
        {
            _slotData = null;
            itemSpriteRenderer.sprite = null;
        }

        public Tween SizeUp()
        {
            _upSized = true;
            return transform.DOScale(1.2f, 0.3f).SetEase(Ease.OutBack);
        }

        public Tween SizeDownToNormal()
        {
            _upSized = false;
            return transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack);
        }

        public void SetSampleList(SampleList instance)
        {
            _sampleList = instance;
        }
    }
}