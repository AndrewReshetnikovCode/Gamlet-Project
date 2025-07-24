using Assets.InventorySystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem
{
    [CreateAssetMenu]
    public class ItemsDatabase : ScriptableObject
    {
        public static ItemsDatabase instance => ItemsManager.ItemsDatabase;
        public List<Item> items;
        
        public static Item Find(string name) => instance.items.Find(i => i.name == name);
    }
}