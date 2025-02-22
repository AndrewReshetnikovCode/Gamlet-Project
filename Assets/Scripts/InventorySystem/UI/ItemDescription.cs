using InventorySystem;
using UnityEngine;


public class ItemDescription : MonoBehaviour, IDescriptionOwner
{
    ItemsCollection _collection;
    public string GetDescription()
    {
        ItemSlot itemSlot = GetComponentInChildren<ItemSlot>();
        _collection = itemSlot.ItemsCollection;
        Item item = _collection[itemSlot.numInContainer].item;
        if (item == null)
        {
            return string.Empty;
        }
        else
        {
            return item.description;
        }
    }

}
