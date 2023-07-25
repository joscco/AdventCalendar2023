using UnityEngine;

namespace GameScene.Grid.Entities.ItemInteraction.Stackable
{
    // A shelf can only hold up a single item
    public class SimpleShelf : InteractableItem
    {
        [SerializeField] private InteractableItem itemOnShelf;


        public override bool CanBeToppedWithItem(InteractableItem item)
        {
            return !IsBearingItem();
        }

        public override InteractableItem GetItem()
        {
            return itemOnShelf;
        }

        public override void TopWithItem(InteractableItem item)
        {
            this.itemOnShelf = item;
            item.AttachToPickupPoint(GetItemHolder());
        }

        public override bool IsBearingItem()
        {
            return null != itemOnShelf;
        }

        public override void RemoveItem(InteractableItem item)
        {
            this.itemOnShelf = null;
        }

        public override bool IsComplete()
        {
            return false;
        }
    }
}