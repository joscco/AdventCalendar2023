using Code.GameScene.Items.Item;
using DG.Tweening;
using GameScene;
using TMPro;
using UnityEngine;

namespace Code.GameScene.Inventory.Renderer
{
    public class InventorySlotInstance : MonoBehaviour
    {
        private InventorySlot _slotInfo;

        [SerializeField] private TextMeshPro counterText;
        [SerializeField] private SpriteRenderer itemSpriteRenderer;

        private InventoryInstance _inventoryInstance;

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
            Debug.Log("Clicked!");
            _inventoryInstance.SelectItemSlot(this);
        }

        public Tween UpdateSlot(InventorySlot newSlotInfo)
        {
            if (null == newSlotInfo)
            {
                return BlendOutThenErase();
            }
            else
            {
                var oldSlotInfo = _slotInfo;
                _slotInfo = newSlotInfo;
                
                if (_shown)
                {
                    if (oldSlotInfo == null || newSlotInfo.Amount != oldSlotInfo.Amount ||
                        newSlotInfo.ItemType != oldSlotInfo.ItemType)
                    {
                        return WiggleAndUpdate(newSlotInfo);
                    }

                    return null;
                }

                return UpdateThenBlendIn(newSlotInfo);
            }
        }

        public void InstantUpdateSlot(InventorySlot newSlotInfo)
        {
            if (null == newSlotInfo)
            {
                transform.localScale = Vector3.zero;
                EraseData();
            }
            else
            {
                transform.localScale = Vector3.one;
                ChangeItemSprite(newSlotInfo.ItemType);
                ChangeNumber(newSlotInfo.Amount);
            }
        }

        public PlantType GetItemType()
        {
            return _slotInfo.ItemType;
        }
        
        public int GetCount()
        {
            return _slotInfo == null ? 0 : _slotInfo.Amount;
        }

        private Tween UpdateThenBlendIn(InventorySlot newSlotInfo)
        {
            ChangeItemSprite(newSlotInfo.ItemType);
            ChangeNumber(newSlotInfo.Amount);
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

        private Tween WiggleAndUpdate(InventorySlot newSlotInfo)
        {
            float preScale = _inventoryInstance.GetSelectedSlot() == this ? 0.85f : 0.75f;
            float postScale = _inventoryInstance.GetSelectedSlot() == this ? 1.2f : 1f;
            
            var sequence = DOTween.Sequence();
            sequence.Append(transform.DOScale(preScale, 0.2f)).SetEase(Ease.OutBack);
            sequence.Append(transform.DOScale(postScale, 0.2f)).SetEase(Ease.OutBack);
            sequence.AppendCallback(() =>
            {
                ChangeItemSprite(newSlotInfo.ItemType);
                ChangeNumber(newSlotInfo.Amount);
            });
            sequence.Play();

            return sequence;
        }

        private void ChangeNumber(int newAmount)
        {
            counterText.text = newAmount.ToString();
        }

        private void ChangeItemSprite(PlantType newItemType)
        {
            itemSpriteRenderer.sprite = Level.Get().plantWiki.GetInventoryIconSpriteForPlant(newItemType);
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
            _slotInfo = null;
            counterText.text = "";
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

        public void SetInventory(InventoryInstance instance)
        {
            _inventoryInstance = instance;
        }
    }
}