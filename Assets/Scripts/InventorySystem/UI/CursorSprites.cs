using DemiurgEngine.UI.Dragging;
using InventorySystem;
using UnityEngine;

[CreateAssetMenu]
public class CursorSprites : GetCursorSpriteBase
{
    const string _traderSlotTag = "TraderSlot";

    [SerializeField] Sprite _buy;
    [SerializeField] Sprite _sold;
    [SerializeField] Sprite _unable;
    public override Sprite GetSprite()
    {
        return _default;
        GameObject underCursor;
        UIDetectUtil.TryGetUIElementUnderCursor(out underCursor);
        if (underCursor.tag == _traderSlotTag)
        {
            if (UIDragHandler.Instance.IsDragging )
            {
                return _sold;
            }
            else
            {
                ItemSlot slot = underCursor.GetComponentInChildren<ItemSlot>();
                ItemEntry itemEntry = slot.ItemEntry;
                if (itemEntry.item == null)
                {
                    return _default;
                }
                int playerMoney = (int)PlayerManager.Instance.Character.stats.GetStat("Money").BaseValue;
                int requiredMoney = itemEntry.item.price;
                if (playerMoney >= requiredMoney) 
                {
                    return _buy;
                }
                else
                {
                    return _unable;
                }
            }
        }
    }
}

