using General.Grid;

namespace GameScene.SpecialGridEntities
{
    public abstract class PickPoint: GridEntity
    {

        public abstract bool CanTakeItem(PickableItem getItem);

        public abstract PickableItem GetItem();
        
        public abstract void GiveItem(PickableItem item);

        public abstract bool HasItemToGive();

        public abstract void RemoveItem(PickableItem item);
    }
}