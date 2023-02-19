using System.Collections.Generic;
using GameScene.Items.Scripts.Item.General;
using UnityEngine;

namespace GameScene.Items.Scripts.Item.Grass
{
    public class GrassElement : FieldEntityElement
    {
        public Sprite noneSprite;
        public Sprite growSprite0;
        public Sprite growSprite1;
        public Sprite finishedSprite;
    
        public override void OnClick()
        {
            if (status == "FINISHED")
            {
                HarvestIfPossible();
            }
        }

        public override InventoryItem GetItem()
        {
            return InventoryItem.Grass;
        }

        protected override Dictionary<InventoryItem, int> GetYieldOnHarvest(string statusOfHarvest)
        {
            return new Dictionary<InventoryItem, int>()
            {
                { InventoryItem.Grass, 2 }
            };
        }

        protected override Sprite GetSpriteForStatus(string requestedStatus)
        {
            switch (requestedStatus)
            {
                case "NONE":
                    return noneSprite;
                case "GROWING_0":
                    return growSprite0;
                case "GROWING_1":
                    return growSprite1;
                case "FINISHED":
                    return finishedSprite;
            }
            return null;
        }

        protected override string GetNextStatus(string requestedStatus)
        {
            switch (requestedStatus)
            {
                case "NONE":
                    return "GROWING_0";
                case "GROWING_0":
                    return "GROWING_1";
                case "GROWING_1":
                    return "FINISHED";
                case "FINISHED":
                    return "FINISHED";
            }
            return "NONE";
        }
    }
}
