using System.Collections.Generic;
using System.Linq;
using GameScene.Grid.Entities.ItemInteraction.Logic.Properties;
using GameScene.Grid.Entities.Shared;
using UnityEngine;

namespace GameScene.Grid.Entities.ItemInteraction.Logic.Checkes
{
    public class MultiTagChecker : GridEntity
    {
        [SerializeField] private List<InteractableItemTag> _demandedTags;
        private bool _wasCheckedSuccessful;

        public void SetDemandedTags(List<InteractableItemTag> tags)
        {
            _demandedTags = tags;
        }

        public void Check(Dictionary<Vector2Int, WordTile> itemMap)
        {
            if (IsSatisfied(itemMap))
            {
                if (!_wasCheckedSuccessful)
                {
                    _wasCheckedSuccessful = true;
                }

                if (itemMap.ContainsKey(currentMainIndex))
                {
                    var item = itemMap[currentMainIndex];
                    OnSatisfied(item);
                }
               
            }
        }


        public bool IsSatisfied(Dictionary<Vector2Int, WordTile> typeMap)
        {
            if (typeMap.ContainsKey(currentMainIndex))
            {
                var item = typeMap[currentMainIndex];
                return !_demandedTags.Except(item.GetItemType().tags).Any();
            }

            return false;
        }
        
        protected void OnSatisfied(WordTile item)
        {
            item.Check();
        }
    }
}