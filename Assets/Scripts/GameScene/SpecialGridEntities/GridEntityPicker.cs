using General.Grid;
using Levels.WizardLevel;
using UnityEngine;

namespace GameScene.PlayerControl
{
    public class GridEntityPicker : MovableGridEntity
    {
        [SerializeField] private Transform itemPositionWhenPickedUp;
        
        private PickableItem pickedItem;

        public bool HasItemToGive()
        {
            return pickedItem != null;
        }
        
        public bool CanTakeItem(PickableItem item)
        {
            return !HasItemToGive();
        }
        
        public PickableItem GetItem()
        {
            return pickedItem;
        }
        
        public void GiveItem(PickableItem item)
        {
            pickedItem = item;
        }
        
        public void RemoveItem(PickableItem item)
        {
            pickedItem = null;
        }

        public Transform GetPickupPoint()
        {
            return itemPositionWhenPickedUp;
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