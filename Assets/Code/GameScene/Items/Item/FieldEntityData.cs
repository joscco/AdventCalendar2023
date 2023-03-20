using System.Collections.Generic;
using Code.GameScene.Inventory;
using UnityEngine;

namespace Code.GameScene.Items.Item
{
    [CreateAssetMenu(fileName = "New FieldEntityData", menuName = "FieldEntityData")]
    public class FieldEntityData : ScriptableObject
    {
        public InventoryItemType itemType;
        public Sprite iconSprite;

        public int secondsPerGrowingStage;
        // Number of Sprites in this list is number of possible stages - 1
        public List<Sprite> growingSprites; 
        public Sprite finishedSprite;
    }
}