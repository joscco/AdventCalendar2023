using UnityEngine;
using UnityEngine.Serialization;

namespace GameScene.Items.Item
{
    [CreateAssetMenu(fileName = "New ItemData", menuName = "ItemData")]
    public class ItemData : ScriptableObject
    {
        public ItemType itemType;
        public Sprite samplesIconSprite;
        public Sprite sprite;
    }
}