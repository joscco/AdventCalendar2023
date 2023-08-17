using System;
using System.Collections.Generic;
using GameScene.Facts;
using GameScene.Grid.Entities.Shared;
using UnityEngine;

namespace GameScene.Grid.Entities.ItemInteraction.Logic.Checkes
{
    public abstract class Checker : GridEntity
    {
        public Action<Fact> OnFirstSuccessfulCheck;
        public bool isImportantForWin = false;
        
        [SerializeField] private Fact factPublishedOnFirstSuccessfulCheck;
        
        private bool wasCheckedSuccessful = false;
        

        public void Check(Dictionary<Vector2Int, InteractableItem> itemMap)
        {
            if (IsSatisfied(itemMap) && (null != factPublishedOnFirstSuccessfulCheck))
            {
                if (!wasCheckedSuccessful)
                {
                    wasCheckedSuccessful = true;
                    OnFirstSuccessfulCheck?.Invoke(factPublishedOnFirstSuccessfulCheck);
                }

                if (itemMap.ContainsKey(currentMainIndex))
                {
                    var item = itemMap[currentMainIndex];
                    OnSatisfied(item);
                }
               
            }
        }

        protected virtual void OnSatisfied(InteractableItem item)
        {
            
        }

        public abstract bool IsSatisfied(Dictionary<Vector2Int, InteractableItem> typeMap);

        public bool HasFactToPublish()
        {
            return null != factPublishedOnFirstSuccessfulCheck && null != factPublishedOnFirstSuccessfulCheck.id;
        }
    }
}