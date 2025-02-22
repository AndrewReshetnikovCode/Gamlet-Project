using DemiurgEngine.UI.Dragging;
using InventorySystem;
using UnityEngine;
using UnityEngine.UI;

public class UIInventoryManager : MonoBehaviour
{
    public static UIInventoryManager instance;

    [SerializeField] ItemsCollection _playerItemsCollection;
    [SerializeField] ItemsCollection _playerEquipCollection;
    [SerializeField] ItemsCollection _playerSymbolCollection;
    [SerializeField] ItemsCollection _traderCollection;
    [SerializeField] Transform _playerInventroyContainer;

    protected UIDragHandler _dragHandler;

    protected ItemEntry _draggedItem;
    protected RectTransform _draggedImage;



    public ItemsCollection Collection { get => _playerItemsCollection; set => _playerItemsCollection = value; }

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        _dragHandler = UIDragHandler.Instance;


        _dragHandler.onStartDrag += OnDragStarted;
        _dragHandler.onEndDrag += OnDragEnd;
    }
    protected virtual void OnDragStarted(DragEventInfo e)
    {
        ItemEntry i;

        IUIDraggable draggableComponent = e.objectUnderCursor.GetComponent<IUIDraggable>();
        if (draggableComponent != null && e.objectUnderCursor.tag != "TraderSlot")
        {
            draggableComponent.GetRect().GetComponent<Image>().sprite = null;
        }

        if (e.objectUnderCursor.tag == "InventorySlot")
        {
            i = _playerItemsCollection[e.slotUnderCursorNum];
            if (i.item == null)
            {
                _dragHandler.IsDragging = false;
                return;
            }
            _dragHandler.SetDraggedSprite(i.item.Sprite);
            _dragHandler.SetDraggedItem(i);
            _playerItemsCollection.Remove(i);

        }

        if (e.objectUnderCursor.tag == "TraderSlot")
        {
            i = _traderCollection[e.slotUnderCursorNum];
            if (i.item == null)
            {
                _dragHandler.IsDragging = false;
                return;
            }
            if (AbleToBuyItem(i.item))
            {
                PlayerManager.StCharacter.AddCurrency(i.item.currencyType, -i.item.price);

                _dragHandler.SetDraggedItem(i);
                _dragHandler.SetDraggedSprite(i.item.Sprite);
            }
        }

        if (e.objectUnderCursor.tag == "EquipSlot")
        {
            i = _playerEquipCollection[e.slotUnderCursorNum];
            Item item = i.item;
            if (i.item == null)
            {
                _dragHandler.IsDragging = false;
                return;
            }
            _dragHandler.SetDraggedItem(i);
            _dragHandler.SetDraggedSprite(i.item.Sprite);
            _playerEquipCollection.Remove(i);

            if (item.upgrade != null && PlayerManager.StCharacter.upgrade.HasUpgrade(item.upgrade))
            {
                PlayerManager.StCharacter.upgrade.RemoveUpgrade(item.upgrade);

            }
        }

        if (e.objectUnderCursor.tag == "SymbolSlot")
        {
            i = _playerSymbolCollection[e.slotUnderCursorNum];
            Item item = i.item;
            if (i.item == null)
            {
                _dragHandler.IsDragging = false;
                return;
            }
            _dragHandler.SetDraggedItem(_playerSymbolCollection[e.slotUnderCursorNum]);
            _dragHandler.SetDraggedSprite(_playerSymbolCollection[e.slotUnderCursorNum].item.Sprite);
            _playerSymbolCollection.Remove(e.slotUnderCursorNum);

            if (item.upgrade != null && PlayerManager.StCharacter.upgrade.HasUpgrade(item.upgrade))
            {
                PlayerManager.StCharacter.upgrade.RemoveUpgrade(item.upgrade);

            }
        }

    }
    protected virtual void OnDragEnd(DragEventInfo e)
    {
        if (e.objectUnderCursor == null)
        {
            return;
        }

        ItemEntry draggedItemEntry = e.itemEntry;
        ItemEntry landedItemEntry = null;

        if (draggedItemEntry != null)
        {
            if (e.objectUnderCursor.tag == "InventorySlot")
            {
                landedItemEntry = _playerItemsCollection[e.slotUnderCursorNum];

                _playerItemsCollection[e.slotUnderCursorNum] = draggedItemEntry;
            }

            if (e.objectUnderCursor.tag == "EquipSlot")
            {
                if (e.itemEntry.item.isSymbol == true)
                {
                    _playerItemsCollection.TryAdd(e.itemEntry);
                    return;
                }
                landedItemEntry = _playerEquipCollection[e.slotUnderCursorNum];

                _playerEquipCollection[e.slotUnderCursorNum] = draggedItemEntry;

                if (landedItemEntry.item != null)
                {
                    if (landedItemEntry.item.upgrade != null && PlayerManager.StCharacter.upgrade.HasUpgrade(landedItemEntry.item.upgrade))
                    {
                        PlayerManager.StCharacter.upgrade.RemoveUpgrade(landedItemEntry.item.upgrade);
                    }
                }
                if (e.itemEntry.item.upgrade != null)
                {
                    PlayerManager.StCharacter.upgrade.AddUpgrade(e.itemEntry.item.upgrade);

                }
            }

            if (e.objectUnderCursor.tag == "TraderSlot")
            {
                PlayerManager.StCharacter.AddCurrency(draggedItemEntry.item.currencyType, draggedItemEntry.item.price);
            }

            if (e.objectUnderCursor.tag == "SymbolSlot")
            {
                if (e.itemEntry.item.isSymbol == false)
                {
                    _playerItemsCollection.TryAdd(e.itemEntry);
                    return;
                }
                landedItemEntry = _playerSymbolCollection[e.slotUnderCursorNum];

                _playerSymbolCollection[e.slotUnderCursorNum] = draggedItemEntry;

                if (landedItemEntry.item != null)
                {
                    if (landedItemEntry.item.upgrade != null && PlayerManager.StCharacter.upgrade.HasUpgrade(landedItemEntry.item.upgrade))
                    {
                        PlayerManager.StCharacter.upgrade.RemoveUpgrade(landedItemEntry.item.upgrade);
                    }
                }
                if (e.itemEntry.item.upgrade != null)
                {
                    PlayerManager.StCharacter.upgrade.AddUpgrade(e.itemEntry.item.upgrade);

                }
            }


            if (landedItemEntry != null && landedItemEntry.item != null)
            {
                _dragHandler.SetDraggedSprite(landedItemEntry.item.Sprite);
                _dragHandler.SetDraggedItem(landedItemEntry);
            }

        }



    }
    bool AbleToBuyItem(Item item)
    {
        return item.price <= PlayerManager.Instance.Character.GetCurrency(item.currencyType);
    }
}
