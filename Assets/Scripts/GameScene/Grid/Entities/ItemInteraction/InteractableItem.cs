using GameScene.PlayerControl;
using GameScene.SpecialGridEntities.PickPoints;
using UnityEngine;

namespace GameScene.Grid.Entities.ItemInteraction
{
    public abstract class InteractableItem : MovableGridEntity, IInteractableItemBearer
    {
        [SerializeField] private Transform itemHolder;
        [SerializeField] private bool pickable;
        [SerializeField] private bool pushable;
        [SerializeField] private InteractableItemType type;

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

        public Transform GetItemHolder()
        {
            return itemHolder;
        }

        public abstract bool CanBeToppedWithItem(InteractableItem item);

        public abstract InteractableItem GetItem();

        public abstract void TopWithItem(InteractableItem item);

        public abstract bool IsBearingItem();

        public abstract void RemoveItem(InteractableItem item);

        public abstract bool IsComplete();
    }
}