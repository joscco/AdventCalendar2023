using GameScene.Grid.Entities.Shared;
using GameScene.SpecialGridEntities.PickPoints;
using UnityEngine;

namespace GameScene.Grid.Entities.ItemInteraction
{
    public abstract class InteractableItem : MovableGridEntity, IInteractableItemBearer
    {
        [SerializeField] private Transform itemHolder;
        [SerializeField] private bool pickable;
        [SerializeField] private bool pushable;
        [SerializeField] private bool interactable = true;
        [SerializeField] private InteractableItemType type;

        private bool wasCompletedAlready;

        protected virtual void OnFirstComplete()
        {
            
        }
        
        protected virtual void OnEachComplete()
        {
            
        }

        protected virtual void OnTopWithItem(InteractableItem item)
        {
            
        }

        public void AttachToPickupPoint(Transform newParent)
        {
            SetParent(newParent);
            LocalMoveTo(Vector2.zero);
        }

        public InteractableItemType GetItemType()
        {
            return type;
        }

        private void SetParent(Transform newParent)
        {
            transform.parent = newParent;
        }

        public bool IsPickable()
        {
            return pickable;
        }

        public bool IsPushable()
        {
            return pushable;
        }

        public bool IsInteractable()
        {
            return interactable;
        }
        
        protected void SetPickable(bool val)
        {
            pickable = val;
        }

        protected void SetPushable(bool val)
        {
            pushable = val;
        }

        protected void SetInteractable(bool val)
        {
            interactable = val;
        }

        public Transform GetItemHolder()
        {
            return itemHolder;
        }

        public abstract bool CanBeToppedWithItem(InteractableItem item);

        public abstract InteractableItem GetItem();

        public void TopWithItem(InteractableItem item)
        {
            OnTopWithItem(item);

            if (IsComplete())
            {
                if (!wasCompletedAlready)
                {
                    wasCompletedAlready = true;
                    OnFirstComplete();
                }
                OnEachComplete();
            }
        }

        public abstract bool IsBearingItem();

        public abstract void RemoveItem(InteractableItem item);

        public abstract bool IsComplete();
    }
}