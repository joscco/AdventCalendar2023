using System.Collections.Generic;
using DG.Tweening;
using GameScene.Items.Item;
using UnityEngine;

namespace GameScene.Items.Field
{
    public class FieldSpotInstance : MonoBehaviour
    {
        private bool _free;

        [SerializeField] private FieldEntityInstance fieldEntityInstance;

        private FieldGridInstance _fieldGridInstance;

        private void Start()
        {
            InstantBlendOut();
        }

        public void UpdateFieldSpot(PlantData data)
        {
            fieldEntityInstance.UpdateFieldEntity(data);
        }

        public void InstantUpdateFieldSpot(PlantData data)
        {
            fieldEntityInstance.InstantUpdateFieldEntity(data);
        }

        public Tween BlendOut()
        {
            return transform.DOScale(0, 0.3f).SetEase(Ease.InBack);
        }

        public Tween BlendIn()
        {
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