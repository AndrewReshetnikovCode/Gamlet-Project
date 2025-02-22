using InventorySystem;
using System.Collections;
using UnityEngine;

public class ChestController : MonoBehaviour
{
//    [SerializeField] private Color highlightColor = Color.yellow;  // ÷вет подсветки, настраиваемый в инспекторе


//    [SerializeField] private Animator chestAnimator;
//    [SerializeField] private GameObject itemPrefab;
//    [SerializeField] private Transform itemSpawnPoint;
//    [SerializeField] private LootTable lootTable;
//    [SerializeField] private int numberOfItemsToSpawn = 1;
//    [SerializeField] private float itemJumpForce = 5f;
//    [SerializeField] private float scatter = 1f;
    
//    private bool isOpened = false;
//    private GameObject spawnedItem;
    

//    private void OnMouseDown()
//    {
//        if (isOpened) return;

//        OpenChest();
//    }

//    public void OpenChest()
//    {
//        isOpened = true;
//        chestAnimator.SetTrigger("Open");
//    }

//    // This method is called by an animation event at the end of the open animation
//    public void SpawnItem()
//    {
//        StartCoroutine(SpawnItemsCoroutine());
//    }
//    private IEnumerator SpawnItemsCoroutine()
//    {
//        for (int i = 0; i < numberOfItemsToSpawn; i++)
//        {
//            Item item = lootTable.GetRandomItem();
//            if (item != null)
//            {
//                GameObject spawnedItem = Instantiate(itemPrefab, itemSpawnPoint.position, Quaternion.identity);
//                spawnedItem.GetComponent<ItemController>().Initialize(item);
//                Rigidbody itemRb = spawnedItem.GetComponent<Rigidbody>();
//                itemRb.AddForce((Vector3.up + Random.onUnitSphere * scatter) * itemJumpForce, ForceMode.Impulse); // Apply force to make the item jump out
//                spawnedItem.GetComponent<ItemController>().StartCoroutineAttractionItem(spawnedItem);
//            }
//            yield return new WaitForSeconds(0.1f); // Small delay between spawning items
//        }

//        //MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
//        //if (meshRenderer != null)
//        //{
//        //    Destroy(meshRenderer);
//        //}
//        Destroy(gameObject); // Destroy chest after spawning items
//    }
//}

//[System.Serializable]
//public class LootTable
//{
//    [SerializeField] private Item[] items;

//    public Item GetRandomItem()
//    {
//        if (items.Length == 0) return null;
//        int randomIndex = Random.Range(0, items.Length);
//        return items[randomIndex];
//    }
}