using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace GameScene.Grid.Entities.ItemInteraction.Logic.Properties
{
    [CreateAssetMenu(fileName = "ItemType", menuName = "Item/Type")]
    public class InteractableItemType : ScriptableObject
    {
        public Sprite itemIcon;
        public float additionalVerticalOffsetImage;
        public LocalizedString title;
        public float additionalVerticalOffsetTitle;
        public string defaultName;
        public List<InteractableItemTag> tags;
    }
}