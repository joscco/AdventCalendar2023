using System.Collections.Generic;
using UnityEngine;

namespace GameScene.Items.Item
{
    public class ItemWiki: MonoBehaviour
    {
        public List<ItemData> entities;
        
        public ItemData GetItemDataForType(ItemType itemType)
        {
            return entities.Find(entity => entity.itemType == itemType);
        }
    
        public Sprite GetSamplesIconSpriteForType(ItemType itemType)
        {
            return entities.Find(entity => entity.itemType == itemType).samplesIconSprite;
        }
    }
}