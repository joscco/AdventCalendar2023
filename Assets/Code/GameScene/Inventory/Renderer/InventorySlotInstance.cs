using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Code.GameScene.Inventory.Renderer
{
    public class InventorySlotInstance : MonoBehaviour
    {
        private InventorySlot _slotInfo;
        
        [SerializeField]
        private TextMeshPro counterText;
        [SerializeField]
        private SpriteRenderer itemSpriteRenderer;
        
        private InventoryInstance _inventoryInstance;
        private InventoryItemWiki _wiki;
        
        private bool _shown;

        private void Start()
        {
            transform.localScale = Vector3.zero;
        }

        public void OnMouseUp()
        {
            Debug.Log("Clicked!");
            _inventoryInstance.SelectItemSlot(this);
        }

        public void UpdateSlot(InventorySlot newSlotInfo)
        {
            if (null == newSlotInfo)
            {
                BlendOutThenErase();
            }
            else
            {
                _slotInfo = newSlotInfo;
                if (_shown)
                {
                    WiggleAndUpdate(newSlotInfo);
                }
                else
                {
                    UpdateThenBlendIn(newSlotInfo);
                }
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

        public InventoryItemType GetItemType()
        {
            return _slotInfo.ItemType;
        }
        
        private void UpdateThenBlendIn(InventorySlot newSlotInfo)
        {
            ChangeItemSprite(newSlotInfo.ItemType);
            ChangeNumber(newSlotInfo.Amount);
            BlendIn();
        }
        
        private void BlendOutThenErase()
        {
            var sequence = DOTween.Sequence();
            sequence.Append(BlendOut());
            sequence.AppendCallback(() => EraseData());
            sequence.Play();
        }

        private void WiggleAndUpdate(InventorySlot newSlotInfo)
        {
            var sequence = DOTween.Sequence();
            sequence.Append(transform.DOScale(0.75f, 0.2f)).SetEase(Ease.OutBack);
            sequence.Append(transform.DOScale(1f, 0.2f)).SetEase(Ease.OutBack);
            sequence.AppendCallback(() =>
            {
                ChangeItemSprite(newSlotInfo.ItemType);
                ChangeNumber(newSlotInfo.Amount);
            });
            sequence.Play();
        }
        
        private void ChangeNumber(int newAmount)
        {
            counterText.text = newAmount.ToString();
        }

        private void ChangeItemSprite(InventoryItemType newItemType)
        {
            itemSpriteRenderer.sprite = _wiki.GetInventoryIconSpriteForItem(newItemType);
        }

        private void BlendIn()
        {
            _shown = true;
            transform.DOScale(1, 0.5f).SetEase(Ease.OutBack);
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
            return transform.DOScale(1.2f, 0.3f).SetEase(Ease.OutBack);
        }

        public Tween SizeDownToNormal()
        {
            return transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack);
        }

        public void SetInventory(InventoryInstance instance)
        {
            _inventoryInstance = instance;
        }

        public void SetWiki(InventoryItemWiki instance)
        {
            _wiki = instance;
        }
    }
}