using System.Collections.Generic;
using UnityEngine;

public class InventoryItemWiki: MonoBehaviour
{
    public List<FieldEntity> entities;

    public FieldEntity GetPrefabForItem(InventoryItem item)
    {
        return entities.Find(entity => entity.item == item);
    }
    
    public Sprite GetInventoryIconSpriteForItem(InventoryItem item)
    {
        return entities.Find(entity => entity.item == item).inventoryIcon;
    }
}