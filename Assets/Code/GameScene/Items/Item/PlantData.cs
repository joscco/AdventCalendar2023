using Code.GameScene.Inventory;
using UnityEngine;

namespace Code.GameScene.Items.Item
{
    [CreateAssetMenu(fileName = "New PlantData", menuName = "PlantData")]
    public class PlantData : ScriptableObject
    {
        public PlantType plantType;
        public Sprite inventoryIconSprite;

        // Number of Sprites in this list is number of possible stages - 1
        public Sprite oneSprite;
        public Sprite twoSprite;
        public Sprite threeSprite;
    }
}