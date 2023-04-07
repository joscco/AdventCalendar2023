using UnityEngine;

namespace GameScene.Items.Item
{
    [CreateAssetMenu(fileName = "New PlantData", menuName = "PlantData")]
    public class PlantData : ScriptableObject
    {
        public PlantType plantType;
        public Sprite inventoryIconSprite;
        public Sprite sprite;
    }
}