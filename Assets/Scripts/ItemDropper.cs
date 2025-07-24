using InventorySystem;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropper : MonoBehaviour
{
    public bool randomDir;
    [SerializeField] bool _wasDrop = false;
    [Header("Настройки")]
    public List<ItemEntry> itemsToDrop;
    public Transform dropPointAndDir;
    public float dropForce = 5f;

    [Header("Анимация")]
    public Animator animator;

    private static readonly int DropTrigger = Animator.StringToHash("Drop");

    void Start()
    {
        if (dropPointAndDir == null)
        {
            dropPointAndDir = transform;
        }
    }
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
            SpawnItem(item);

        }
    }

    /// <summary>
    /// Спавнит один предмет из данных.
    /// </summary>
    /// <param name="item">Данные о предмете.</param>
    private void SpawnItem(ItemEntry item)
    {
        Vector3 dir;
        if (randomDir)
        {
            dir = new Vector3();
            dir.x = Random.Range(-1f, 1f);
            dir.z = Random.Range(-1f, 1f);
        }
        else
        {
            dir = dropPointAndDir.forward;
            dir.y = 0;
        }
        Instantiate(item.item.Prefab, dropPointAndDir.position + Vector3.up, Quaternion.LookRotation(dir)).GetComponentInChildren<WorldItemController>().ItemEntry = item;
        //ItemDropUtil.Drop(dropPointAndDir.position, force, item);
    }
}
