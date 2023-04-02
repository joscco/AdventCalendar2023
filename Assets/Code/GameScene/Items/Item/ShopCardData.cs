using Code.GameScene.Inventory;
using UnityEngine;

namespace Code.GameScene.Items.Item
{
    [CreateAssetMenu(fileName = "New ShopCardData", menuName = "CardData")]
    public class ShopCardData : ScriptableObject
    {
        public PlantType plantType;
        public Sprite cardSprite;
        public int buyPrice;
        public int sellPrice;
    }
}