using System;
using System.Collections.Generic;
using GameScene.Facts;
using GameScene.Grid.Entities.ItemInteraction.Logic.Properties;
using GameScene.Grid.Entities.Shared;
using UnityEngine;

namespace GameScene.Grid.Entities.ItemInteraction.Logic.Checkes
{
    public abstract class Checker : GridEntity
    {
        public Action<Fact> OnFirstSuccessfulCheck;

        [SerializeField] private Fact factPublishedOnFirstSuccessfulCheck;
        
        private bool wasCheckedSuccessful = false;
        protected Vector2Int index;

        public void Check(Dictionary<Vector2Int, InteractableItemType> typeMap)
        {
            if (IsSatisfied(typeMap) && (null != factPublishedOnFirstSuccessfulCheck))
            {
                if (!wasCheckedSuccessful)
                {
                    wasCheckedSuccessful = true;
                    OnFirstSuccessfulCheck?.Invoke(factPublishedOnFirstSuccessfulCheck);
                }
                
            }
        }

        public abstract bool IsSatisfied(Dictionary<Vector2Int, InteractableItemType> typeMap);

        public void SetIndex(Vector2Int index)
        {
            this.index = index;
        }
    }
}