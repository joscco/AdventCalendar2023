using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameScene.Items.Scripts.Item.General
{
    public abstract class FieldEntityElement : MonoBehaviour
    {
        public string status = "NONE";
        public FieldEntityElementRenderer elementRenderer;

        public void Evolve()
        {
            status = GetNextStatus(status);
            Sprite newStatusSprite = GetSpriteForStatus(status);
            elementRenderer.UpdateSpriteWhileBlendIn(newStatusSprite);
        }

        public void BlendIn()
        {
            elementRenderer.BlendIn(GetSpriteForStatus(status));
        }
    
        public void BlendOut()
        {
            elementRenderer.BlendOut();
        }

        public abstract void OnClick();

        public abstract InventoryItem GetItem();

        public void HarvestIfPossible()
        {
            // // Is that an Element or rather an Entity check? -> Both actually
            // if (GameManager.Instance.Inventory.CanAddInventoryItems(_item, yieldPerItem))
            // {
            //     GetYieldOnHarvest(status);
            //     GameManager.Instance.Inventory.AddInventoryItems(_item, yieldPerItem);
            //     status = "NONE";
            //     StartCoroutine(StartDestruction());
            // }
        }

        private IEnumerator StartDestruction()
        {
            yield return elementRenderer.BlendOut();
        }

        protected abstract Dictionary<InventoryItem, int> GetYieldOnHarvest(string statusOfHarvest);
    
        protected abstract Sprite GetSpriteForStatus(string requestedStatus);

        protected abstract string GetNextStatus(string requestedStatus);
    }
}