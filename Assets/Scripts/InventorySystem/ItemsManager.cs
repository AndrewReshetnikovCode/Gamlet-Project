using InventorySystem;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Assets.InventorySystem
{
    public class ItemsManager : MonoBehaviour
    {
        public static ItemsManager instance;

        public static ItemsDatabase ItemsDatabase => instance.itemsDatabase;
        public ItemsDatabase itemsDatabase;
        private void Awake()
        {
            instance = this;

            ItemsManager founded = GameObject.FindObjectOfType<ItemsManager>();
            if (founded != null && founded != this)
            {
                Destroy(gameObject);
                return;
            }
            if (itemsDatabase == null)
            {
                //IEnumerator e = Unity.VisualScripting.AssetUtility.GetAllAssetsOfType<ItemsDatabase>().GetEnumerator();
                //e.MoveNext();
                //itemsDatabase = (ItemsDatabase)e.Current;
            }
            DontDestroyOnLoad(gameObject);
        }
    }
}