using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Serialization;

namespace GameScene.Grid.Entities.ItemInteraction.Logic.Properties
{
    [CreateAssetMenu(fileName = "ItemType", menuName = "Item/Type")]
    public class InteractableItemType : ScriptableObject
    {
        public Sprite itemIcon;
        public LocalizedString title;
        public string defaultName;
        public List<InteractableItemTag> tags;
    }
}