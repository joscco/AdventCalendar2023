using System.Collections.Generic;
using Code.GameScene.Inventory;
using Code.GameScene.Items.Item;
using UnityEngine;

namespace Code
{
    public class InventoryItemWiki: MonoBehaviour
    {
        public List<PlantData> entities;
        
        public PlantData GetFieldEntityDataForItem(PlantType itemType)
        {
            return entities.Find(entity => entity.plantType == itemType);
        }
    
        public Sprite GetInventoryIconSpriteForItem(PlantType itemType)
        {
            return entities.Find(entity => entity.plantType == itemType).inventoryIconSprite;
        }
    }
}