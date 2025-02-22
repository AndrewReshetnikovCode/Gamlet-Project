using DemiurgEngine.UI.Dragging;
using UnityEngine;

namespace InventorySystem
{
    public class ItemsListView : MonoBehaviour
    {
        [SerializeField] protected ItemsCollection _collection;
        [SerializeField] protected Transform _container;

        protected UIDragHandler _dragHandler;
        public ItemsCollection Collection { get; set; }

        protected Item _draggedItem;
        protected RectTransform _draggedImage;
        private void Start()
        { 
            _dragHandler = UIDragHandler.Instance;  
            FillByCollection();
            for (int i = 0; i < _collection.Count; i++)
            {
                ItemSlot slot = _container.GetChild(i).GetComponentInChildren<ItemSlot>();
                slot.ItemsCollection = _collection;
            }

            _collection.onChange += FillByCollection;
        }
        
        void OnEnable()
        {
            FillByCollection();
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
        bool AbleToAddItem(Item item, int slotNum)
        {
            return _collection.CanAdd(item, 1, slotNum);
        }
        void OnItemSlotClicked(ItemSlotEvent e)
        {

        }
        public void SetItemCollection(ItemsCollection c)
        {
            Collection = c;
        }
        public void Display(bool v)
        {
            for (int i = 0; i < _container.childCount; i++)
            {
                if (i >= Collection.Count)
                {
                    break;
                }
                _container.GetChild(i).GetComponent<ItemSlot>().Set(Collection.GetItemAt(i).item.Sprite);
            }
        }
    }
}
