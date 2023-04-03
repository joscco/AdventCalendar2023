using System.Collections.Generic;
using Code.GameScene.Inventory;
using Code.GameScene.Items.Item;
using UnityEngine;

namespace Code
{
    public class PlantWiki: MonoBehaviour
    {
        public List<PlantData> entities;
        
        public PlantData GetPlantDataForPlant(PlantType itemType)
        {
            return entities.Find(entity => entity.plantType == itemType);
        }
    
        public Sprite GetInventoryIconSpriteForPlant(PlantType itemType)
        {
            return entities.Find(entity => entity.plantType == itemType).inventoryIconSprite;
        }
    }
}