using System.Collections.Generic;
using Code.GameScene.Inventory;
using Code.GameScene.Items.Item;
using DG.Tweening;
using UnityEngine;

namespace Code.GameScene.Items.Field
{
    public class FieldSpotInstance : MonoBehaviour
    {
        private int row;
        private int column;
        private bool free;

        [SerializeField] private SpriteRenderer fieldSpriteRenderer;
        [SerializeField] private FieldEntityInstance fieldEntityInstance;

        private FieldGridInstance _fieldGridInstance;

        private void Start()
        {
            InstantBlendOut();
            fieldEntityInstance.SetFieldSpot(this);
        }

        public void OnMouseUp()
        {
            Debug.Log("Clicked Spot Advance");
            if (free)
            {
                Debug.Log("Spot Clicked!");
                _fieldGridInstance.OnSpotClick(row, column);
            }
            else
            {
                Debug.Log("Field Clicked!");
                _fieldGridInstance.OnFieldEntityClick(row, column);
            }
        }

        public void SetRowAndColumn(int newRow, int newColumn)
        {
            row = newRow;
            column = newColumn;
        }

        public void UpdateFieldSpot(FieldEntityData data)
        {
            if (null == data)
            {
                free = true;
                fieldSpriteRenderer.transform.DOScale(1, 0.3f).SetEase(Ease.OutBack);
            }
            else
            {
                free = false;
                fieldSpriteRenderer.transform.DOScale(0, 0.3f).SetEase(Ease.InBack);
            }

            fieldEntityInstance.UpdateFieldEntity(data);
        }

        public void InstantUpdateFieldSpot(FieldEntityData data)
        {
            if (null == data)
            {
                free = true;
                fieldSpriteRenderer.transform.localScale = Vector3.one;
            }
            else
            {
                free = false;
                fieldSpriteRenderer.transform.localScale = Vector3.zero;
            }

            fieldEntityInstance.InstantUpdateFieldEntity(data);
        }

        public Tween BlendOut()
        {
            return transform.DOScale(0, 0.3f).SetEase(Ease.InBack);
        }

        public Tween BlendIn()
        {
            Debug.Log("Blend in!");
            return transform.DOScale(1, 0.3f).SetEase(Ease.OutBack);
        }

        public void InstantBlendOut()
        {
            transform.localScale = Vector3.zero;
        }

        public void InstantBlendIn()
        {
            transform.localScale = Vector3.one;
        }

        public void SetGridInstance(FieldGridInstance fieldGridInstance)
        {
            _fieldGridInstance = fieldGridInstance;
            fieldEntityInstance.SetGrid(fieldGridInstance);
        }

        public bool IsFree()
        {
            return free;
        }

        public bool CanHarvest()
        {
            return fieldEntityInstance.CanHarvest();
        }

        public Dictionary<InventoryItemType, int> GetHarvest()
        {
            return fieldEntityInstance.Harvest();
        }

        public Tween DoMoveTo(Vector3 pos)
        {
            return transform.DOMove(pos, 0.3f).SetEase(Ease.InOutBack);
        }
    }
}