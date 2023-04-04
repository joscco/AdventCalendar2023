using Code.GameScene.Items.Item;

namespace Code.GameScene.Inventory
{
    public class InventorySlot
    {
        public InventorySlot(PlantType itemType, int amount)
        {
            Amount = amount;
            ItemType = itemType;
        }

        public int Amount;
        public PlantType ItemType;
    }
}