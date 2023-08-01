using System.Collections.Generic;
using GameScene.Grid.Entities.ItemInteraction;
using GameScene.Grid.Entities.Shared;
using UnityEngine;

namespace GameScene.Grid.Entities.Obstacles
{
    public abstract class TemporaryObstacle : GridEntity
    {
        [SerializeField] private bool blocking = true;
        [SerializeField] private List<InteractableItem> itemsToListenToForCompletion;

        public void CheckStatus()
        {
            if (blocking)
            {
                var allItemsComplete = true;
                foreach (var item in itemsToListenToForCompletion)
                {
                    allItemsComplete &= item.IsComplete();
                }

                if (allItemsComplete)
                {
                    blocking = false;
                    OnUnblock();
                }
            }
        }

        public bool IsBlocking()
        {
            return blocking;
        }

        protected abstract void OnUnblock();
    }
}