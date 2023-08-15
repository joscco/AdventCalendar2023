using System.Collections.Generic;
using GameScene.Grid.Entities.ItemInteraction.Logic.Properties;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameScene.Grid.Entities.ItemInteraction.Logic.Checkes
{
    public class FieldTagChecker : Checker
    {
        [SerializeField] private InteractableItemTag demandedTag;
        

        public override bool IsSatisfied(Dictionary<Vector2Int, InteractableItemType> typeMap)
        {
            return typeMap.ContainsKey(index) && typeMap[index].tags.Contains(demandedTag);
        }
    }
}