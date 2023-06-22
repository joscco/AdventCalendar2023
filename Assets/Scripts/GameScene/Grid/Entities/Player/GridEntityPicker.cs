using GameScene.Grid.Entities.Player;
using General.Grid;
using UnityEngine;

namespace GameScene.PlayerControl
{
    public class GridEntityPicker : MovableGridEntity
    {
        [SerializeField] private Transform itemPositionWhenPickedUp;
        [SerializeField] private PickableItem pickedItem;

        public bool HasItemToGive()
        {
            return pickedItem != null;
        }

        public bool CanTakeItem(PickableItem item)
        {
            return !HasItemToGive();
        }

        protected override void UpdateSortingOrder(int order)
        {
            base.UpdateSortingOrder(order);
            if (null != pickedItem)
            {
                pickedItem.SetSortingOrder(order);
            }
        }

        public PickableItem GetItem()
        {
            return pickedItem;
        }

        public void GiveItem(PickableItem item)
        {
            pickedItem = item;
            ((PlayerRenderer)entityRenderer).ShowCarrying();
            item.AttachToPickupPoint(itemPositionWhenPickedUp);
            item.SetSortingOrder(-currentMainIndex.y);
        }

        public void RemoveItem(PickableItem item)
        {
            pickedItem = null;
            ((PlayerRenderer)entityRenderer).ShowIdle();
        }

        public void PlayDeathAnimation()
        {
            Debug.Log("Player Died!");
        }

        public void PlayWinAnimation()
        {
            Debug.Log("Player Won!");
        }
    }
}