namespace GameScene.Inventory.Scripts
{
    public class InventorySlotEntry
    {
        public int Amount;
        public InventoryItem Item;

        public InventorySlotEntry(InventoryItem item, int amount)
        {
            Amount = amount;
            Item = item;
        }
    }
}