using UnityEngine;

namespace InventorySystem
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
    public class Item : ScriptableObject
    {
        [SerializeField] GameObject _prefab;
        [SerializeField] string _itemName;
        [SerializeField] Sprite _sprite;

        [TextArea] public string description;
        public Upgrade upgrade;

        public CurrencyType currencyType;
        public bool isSymbol = false;
        public int price;
        public float damage;
        public string ItemName => _itemName;
        public Sprite Sprite => _sprite;
        public GameObject Prefab => _prefab;

        public GameObject CreateInstanceInWorld()
        {
            return Instantiate(_prefab);
        }
        
    }
}
