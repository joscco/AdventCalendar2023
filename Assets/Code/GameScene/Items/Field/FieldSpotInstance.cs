using System.Collections.Generic;
using Code.GameScene.Inventory;
using Code.GameScene.Items.Item;
using DG.Tweening;
using UnityEngine;

namespace Code.GameScene.Items.Field
{
    public class FieldSpotInstance : MonoBehaviour
    {
        private int _row;
        private int _column;
        
        private bool _free;

        [SerializeField] private FieldEntityInstance fieldEntityInstance;

        private FieldGridInstance _fieldGridInstance;

        private void Start()
        {
            InstantBlendOut();
        }

        public void OnMouseUp()
        {
            Debug.Log("Clicked Spot Advance");
            if (_free)
            {
                Debug.Log("Spot Clicked!");
                _fieldGridInstance.OnSpotClick(_row, _column);
            }
            else
            {
                Debug.Log("Field Clicked!");
                _fieldGridInstance.OnFieldEntityClick(_row, _column);
            }
        }

        public void SetRowAndColumn(int newRow, int newColumn)
        {
            _row = newRow;
            _column = newColumn;
        }

        public void UpdateFieldSpot(PlantData data)
        {
            if (null == data)
            {
                _free = true;
            }
            else
            {
                _free = false;
            }

            fieldEntityInstance.UpdateFieldEntity(data);
        }

        public void InstantUpdateFieldSpot(PlantData data)
        {
            if (null == data)
            {
                _free = true;
                //fieldSpriteRenderer.transform.localScale = Vector3.one;
            }
            else
            {
                _free = false;
                //fieldSpriteRenderer.transform.localScale = Vector3.zero;
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
        }

        public bool IsFree()
        {
            return _free;
        }
        
        public PlantData GetPlantData()
        {
            if (fieldEntityInstance)
            {
                return fieldEntityInstance.GetPlantData();
            }

            return null;
        }

        public bool CanHarvest()
        {
            return fieldEntityInstance.CanHarvest();
        }

        public Dictionary<PlantType, int> GetHarvest()
        {
            return fieldEntityInstance.Harvest();
        }

        public Tween DoMoveTo(Vector3 pos)
        {
            return transform.DOMove(pos, 0.3f).SetEase(Ease.InOutBack);
        }

        public void Evolve()
        {
            if (fieldEntityInstance)
            {
                fieldEntityInstance.Evolve();
            }
        }
    }
}