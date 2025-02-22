using InventorySystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemImage : MonoBehaviour
{
    Image _image;
    ItemSlot _slot;
    void Start() {
    _slot = transform.parent.GetComponentInChildren<ItemSlot>();
        _image = GetComponent<Image>();
    }
    void Update()
    {
        bool slotFilled = _slot.ItemEntry.item != null;
        _image.enabled = slotFilled;
    }
}
