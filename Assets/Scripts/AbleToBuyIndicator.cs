using InventorySystem;
using UnityEngine;
using UnityEngine.UI;


public class AbleToBuyIndicator : MonoBehaviour
{
    [SerializeField] Image _image;
    [SerializeField] ItemSlot _itemSlot;
    void Update()
    {
        bool canBuy = _itemSlot.ItemEntry.item != null && PlayerManager.StCharacter.GetCurrency(_itemSlot.ItemEntry.item.currencyType) >= _itemSlot.ItemEntry.item.price;
        _image.enabled = canBuy;
    }
}
