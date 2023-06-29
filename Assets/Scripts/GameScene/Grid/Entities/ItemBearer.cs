using General.Grid;

namespace GameScene.Grid.Entities
{
    public interface ItemBearer
    {
        public PickableItem GetItem();

        public void GiveItem(PickableItem item);

        public bool HasItemToGive();

        public void RemoveItem(PickableItem item);

        public bool CanTakeItem(PickableItem item);
    }
}