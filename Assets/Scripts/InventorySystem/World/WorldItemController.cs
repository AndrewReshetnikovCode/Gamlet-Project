using InventorySystem;
using UnityEngine;

public class WorldItemController : MonoBehaviour
{
    [SerializeField] ItemEntry _itemEntry;
    public ItemEntry ItemEntry { get => _itemEntry; set => _itemEntry = value; }
    public float unpickableTime;
    public bool pickable;
    public GameObject rootObject;
    private void Start()
    {
        if (_itemEntry.item == null)
        {
            _itemEntry = new();
            _itemEntry.item = ItemsDatabase.instance.items[0];
            _itemEntry.quantity = 1;
        }
        else
        {
            _itemEntry.quantity = 1;
        }
            pickable = false;
        Invoke("SetPickable", unpickableTime);
    }
    public void ResetUnpickableTime()
    {
        pickable = false;
        Invoke("SetPickable", unpickableTime);
    }
    void SetPickable()
    {
        pickable = true;
    }
    private void OnTriggerStay(Collider other)
    {
        if (pickable)
        {
            if (other.transform.tag == "Player")
            {
                bool added = PlayerManager.CharacterStatic.itemsCollection.TryAdd(ItemEntry);
                if (added)
                {
                    Destroy(rootObject);
                }
            }
        }
    }

}
