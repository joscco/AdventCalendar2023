using System.Collections.Generic;
using UnityEngine;

namespace GameScene.SpecialGridEntities.PickPoints
{
    [CreateAssetMenu(fileName = "ItemType", menuName = "Item/Type")]
    public class InteractableItemType : ScriptableObject
    {
        public InteractableItemCategory category;
    }
}