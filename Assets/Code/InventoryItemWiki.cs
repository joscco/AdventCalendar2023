using System.Collections.Generic;
using Code.GameScene.Inventory;
using Code.GameScene.Items.Item;
using UnityEngine;

namespace Code
{
    public class InventoryItemWiki: MonoBehaviour
    {
        public List<FieldEntityData> entities;
        public FieldEntityInstance fieldEntityPrefab;
        
        public FieldEntityData GetFieldEntityDataForItem(InventoryItemType itemType)
        {
            return entities.Find(entity => entity.itemType == itemType);
        }
    
        public Sprite GetInventoryIconSpriteForItem(InventoryItemType itemType)
        {
            return entities.Find(entity => entity.itemType == itemType).iconSprite;
        }
    }
}