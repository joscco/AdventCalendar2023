namespace GameScene.Grid.Entities.ItemInteraction
{
    public interface IInteractableItemBearer
    {
        public InteractableItem GetItem();

        public void TopWithItem(InteractableItem item);

        public bool IsBearingItem();

        public void RemoveItem(InteractableItem item);

        public bool CanBeToppedWithItem(InteractableItem item);
    }
}