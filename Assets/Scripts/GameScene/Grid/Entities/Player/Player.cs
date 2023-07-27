using GameScene.Grid.Entities.ItemInteraction;
using GameScene.Grid.Entities.Shared;
using UnityEngine;

namespace GameScene.Grid.Entities.Player
{
    public class Player : MovableGridEntity, IInteractableItemBearer
    {
        [SerializeField] private Transform itemPositionWhenPickedUp;
        [SerializeField] private InteractableItem pickedItem;

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

        public bool IsBearingItem()
        {
            return pickedItem != null;
        }

        public bool CanBeToppedWithItem(InteractableItem item)
        {
            return !IsBearingItem();
        }

        public InteractableItem GetItem()
        {
            return pickedItem;
        }

        public void TopWithItem(InteractableItem item)
        {
            pickedItem = item;
            ShowCarrying();
            item.AttachToPickupPoint(itemPositionWhenPickedUp);
        }

        public void RemoveItem(InteractableItem item)
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