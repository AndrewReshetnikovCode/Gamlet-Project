using InventorySystem;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropper : MonoBehaviour
{
    [SerializeField] bool _wasDrop = false;
    [Header("Настройки")]
    public List<ItemEntry> itemsToDrop;
    public Transform dropPoint;      
    public float dropForce = 5f;

    [Header("Анимация")]
    public Animator animator;         

    private static readonly int DropTrigger = Animator.StringToHash("Drop");

    /// <summary>
    /// Метод для запуска выпадения предметов.
    /// </summary>
    public void DropItems()
    {
        if (_wasDrop)
        {
            return;
        }
        if (animator)
        {
            animator.SetTrigger(DropTrigger);
        }
        else
        {
            Debug.LogWarning("Animator не назначен на объекте!");
            SpawnItems(); // Если анимации нет, вызываем спавн сразу
        }
        _wasDrop = true;
    }

    /// <summary>
    /// Вызывается анимацией в момент спавна.
    /// </summary>
    public void SpawnItems()
    {
        foreach (var item in itemsToDrop)
        {
            for (int i = 0; i < item.quantity; i++)
            {
                SpawnItem(item);
            }
        }
    }

    /// <summary>
    /// Спавнит один предмет из данных.
    /// </summary>
    /// <param name="item">Данные о предмете.</param>
    private void SpawnItem(ItemEntry item)
    {
        if (item.item.Prefab == null)
        {
            Debug.LogWarning($"Префаб для предмета {item.item} не назначен!");
            return;
        }

        // Создаем объект предмета
        WorldItemController spawnedItem = Instantiate(item.item.Prefab, dropPoint.position, Quaternion.identity);
        
        // Добавляем разброс
        Rigidbody rb = spawnedItem.GetComponent<Rigidbody>();
        if (rb)
        {
            Vector3 force = new Vector3(
                Random.Range(-1f, 1f),
                1f,   
                Random.Range(-1f, 1f)
            ).normalized * dropForce;
            rb.AddForce(force, ForceMode.Impulse);
        }
    }
}
