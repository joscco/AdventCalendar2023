using System.Collections.Generic;
using UnityEngine;

namespace GameScene.Items.Item
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