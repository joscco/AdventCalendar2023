using GameScene.Grid.Entities;
using General.Grid;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameScene.PlayerControl
{
    public class Player : MovableGridEntity, ItemBearer
    {
        [SerializeField] private Transform itemPositionWhenPickedUp;
        [SerializeField] private PickableItem pickedItem;

        [SerializeField] private SpriteRenderer bodySpriteRenderer;
        [SerializeField] private Sprite spriteWhenCarrying;
        [SerializeField] private Sprite spriteWhenNormal;
        
        public void ShowCarrying()
        {
            bodySpriteRenderer.sprite = spriteWhenCarrying;
        }
    
        public void ShowIdle()
        {
            bodySpriteRenderer.sprite = spriteWhenNormal;
        }

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
            ShowCarrying();
            item.AttachToPickupPoint(itemPositionWhenPickedUp);
        }

        public void RemoveItem(PickableItem item)
        {
            pickedItem = null;
            ShowIdle();
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