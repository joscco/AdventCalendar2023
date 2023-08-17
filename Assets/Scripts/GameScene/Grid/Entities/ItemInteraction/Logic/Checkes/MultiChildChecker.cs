using System;
using System.Collections.Generic;
using System.Linq;
using GameScene.Grid.Entities.ItemInteraction.Logic.Properties;
using UnityEngine;

namespace GameScene.Grid.Entities.ItemInteraction.Logic.Checkes
{
    public class MultiChildChecker : Checker
    {
        [SerializeField] private List<Checker> subcheckers;

        public override bool IsSatisfied(Dictionary<Vector2Int, InteractableItem> typeMap)
        {
            return subcheckers.All(checker => checker.IsSatisfied(typeMap));
        }
    }
}