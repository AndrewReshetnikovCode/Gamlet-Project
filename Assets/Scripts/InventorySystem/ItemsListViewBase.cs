using DemiurgEngine.UI.Dragging;
using InventorySystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsListViewBase : MonoBehaviour
{
    [SerializeField] ItemsCollection _collection;
    [SerializeField] Transform _container;

    public ItemsCollection Collection { get => _collection; set => _collection = value; }

    private void Start()
    {
        _collection.onChange += FillByCollection;
    }
    void FillByCollection()
    {
        for (int i = 0; i < _collection.Count; i++)
        {
            ItemEntry e = _collection[i];
            if (_collection[i].item == null)
            {
                _container.GetChild(i).GetComponentInChildren<ItemSlot>().ItemImage.sprite = null;
            }
            else
            {
                Sprite sprite = e.item.Sprite;
                _container.GetChild(i).GetComponentInChildren<ItemSlot>().ItemImage.sprite = sprite;
            }
        }
    }
    int FirstValidSlot(Item item, int quantity)
    {
        return _collection.GetFirstValidSlot(item, quantity);
    }
}

