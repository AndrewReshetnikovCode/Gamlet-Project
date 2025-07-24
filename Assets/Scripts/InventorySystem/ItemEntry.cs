using System;
namespace InventorySystem
{
    [Serializable]
    public class ItemEntry
    {
        public Item item;
        public int quantity;


        public ItemEntry Clone()
        {
            return new ItemEntry() { item = item, quantity = quantity };
        }
    }


}