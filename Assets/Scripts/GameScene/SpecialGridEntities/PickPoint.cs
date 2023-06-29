using GameScene.Grid.Entities;
using GameScene.Grid.Entities.Shared;
using General.Grid;

namespace GameScene.SpecialGridEntities
{
    public abstract class PickPoint: GridEntity, ItemBearer
    {

        public abstract bool CanTakeItem(PickableItem item);

        public abstract PickableItem GetItem();
        
        public abstract void GiveItem(PickableItem item);

        public abstract bool HasItemToGive();

        public abstract void RemoveItem(PickableItem item);

        public abstract bool IsComplete();
    }
}