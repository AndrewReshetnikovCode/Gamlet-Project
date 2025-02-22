using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject itemPrefab;
    public int itemCount = 5;

    void Start()
    {
        SpawnItems();
    }

    void SpawnItems()
    {
        for (int i = 0; i < itemCount; i++)
        {
            Vector3 randomPosition = new Vector3(Random.Range(1, 20), 0, Random.Range(1, 20));
            Instantiate(itemPrefab, randomPosition, Quaternion.identity);
        }
    }
}
