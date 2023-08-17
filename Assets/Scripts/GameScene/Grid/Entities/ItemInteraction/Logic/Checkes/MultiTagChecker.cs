using System;
using System.Collections.Generic;
using System.Linq;
using GameScene.Grid.Entities.ItemInteraction.Logic.Properties;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameScene.Grid.Entities.ItemInteraction.Logic.Checkes
{
    public class MultiTagChecker : Checker
    {
        [SerializeField] private List<InteractableItemTag> demandedTags;


        public override bool IsSatisfied(Dictionary<Vector2Int, InteractableItem> typeMap)
        {
            if (typeMap.ContainsKey(currentMainIndex))
            {
                var item = typeMap[currentMainIndex];
                return !demandedTags.Except(item.GetItemType().tags).Any();
            }

            return false;
        }
        
        protected override void OnSatisfied(InteractableItem item)
        {
            item.Check();
        }
    }
}