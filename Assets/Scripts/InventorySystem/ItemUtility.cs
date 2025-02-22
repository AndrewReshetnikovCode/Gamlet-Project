using InventorySystem;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

public static class ItemUtility
{
    static ItemsCollection _worldItemsCollection;
    public static void Init(ItemsCollection worldItems)
    {
        _worldItemsCollection = worldItems;
    }
    public static void CreateAndAttach(Vector3 pos, ItemEntry entry)
    {
        WorldItemController ic = CreateWorldItem(pos, entry);

    }
    public static WorldItemController CreateWorldItem(Vector3 pos, ItemEntry entry)
    {
        WorldItemController instance = Object.Instantiate(entry.item.Prefab, pos, Quaternion.identity);
        instance.itemEntry = entry;
        return instance;
    }
}