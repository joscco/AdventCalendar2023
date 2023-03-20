namespace Code.GameScene.Inventory
{
    public class InventorySlot
    {
        public InventorySlot(InventoryItemType itemType, int amount)
        {
            Amount = amount;
            ItemType = itemType;
        }

        public int Amount;
        public InventoryItemType ItemType;
    }
}