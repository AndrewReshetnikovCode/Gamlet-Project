using InventorySystem;
using UnityEngine;

public static class ItemDropUtil
{
    public static void Drop(Vector3 pos, Vector3 impulse, ItemEntry item)
    {
        if (item.item.Prefab == null)
        {
            Debug.LogWarning($"������ ��� �������� {item.item} �� ��������!");
            return;
        }

        // ������� ������ ��������
        GameObject spawnedItem = GameObject.Instantiate(item.item.Prefab, pos, Quaternion.identity);

        // ��������� �������
        Rigidbody rb = spawnedItem.GetComponent<Rigidbody>();
        if (rb)
        {
            rb.AddForce(impulse, ForceMode.Impulse);
        }
    }
}