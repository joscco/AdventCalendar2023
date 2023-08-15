using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace GameScene.Grid.Entities.ItemInteraction.Logic.Properties
{
    [CreateAssetMenu(fileName = "ItemType", menuName = "Item/Type")]
    public class InteractableItemType : ScriptableObject
    {
        public Sprite itemIcon;
        public LocalizedString name;
        public List<InteractableItemTag> tags;
    }
}