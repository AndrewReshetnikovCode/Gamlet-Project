using InventorySystem;
using UnityEngine;

public static class ItemDropUtil
{
    public static void Drop(Vector3 pos, Vector3 impulse, ItemEntry item)
    {
        if (item.item.Prefab == null)
        {
            Debug.LogWarning($"Префаб для предмета {item.item} не назначен!");
            return;
        }

        // Создаем объект предмета
        GameObject spawnedItem = GameObject.Instantiate(item.item.Prefab, pos, Quaternion.identity);

        // Добавляем разброс
        Rigidbody rb = spawnedItem.GetComponent<Rigidbody>();
        if (rb)
        {
            rb.AddForce(impulse, ForceMode.Impulse);
        }
    }
}