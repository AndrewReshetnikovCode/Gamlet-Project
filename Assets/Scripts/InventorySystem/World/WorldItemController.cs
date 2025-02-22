using InventorySystem;
using UnityEngine;


public class WorldItemController : MonoBehaviour
{
    public ItemEntry itemEntry;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            bool added = PlayerManager.StCharacter.itemsCollection.TryAdd(itemEntry);
            if (added)
            {
                Destroy(gameObject);
            }
        }
    }
}
