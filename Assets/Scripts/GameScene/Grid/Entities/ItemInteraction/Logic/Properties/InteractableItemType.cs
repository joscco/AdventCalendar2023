using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace GameScene.Grid.Entities.ItemInteraction.Logic.Properties
{
    [CreateAssetMenu(fileName = "ItemType", menuName = "Item/Type")]
    public class InteractableItemType : ScriptableObject
    {
        public Sprite itemIcon;
        public Vector2 additionalOffsetImage;
        public LocalizedString title;
        public Vector2 additionalOffsetTitle;
        [TextArea(3, 10)]
        public string defaultName;
        public List<InteractableItemTag> tags;
    }
}