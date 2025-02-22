using DemiurgEngine.UI.Dragging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace InventorySystem
{

    public class ItemSlotEvent
    {
        //HACK
        public int slotNum => slot.numInContainer;
        public bool shiftPressed = false;
        public bool mouseDown = false;
        public ItemSlot slot;
        public object slotInfo;
        public PointerEventData pointerEventData;
    }
    public class ItemSlot : MonoBehaviour, IUIDraggable, IDragLandable
    {
        public event Action<ItemSlotEvent> onUIEvent;


        public RectTransform RectTransform => GetComponent<RectTransform>();
        public Image ItemImage;
        [SerializeField] GameObject _highlightImage;
        [SerializeField] GameObject _selectImage;

        public ItemEntry ItemEntry => ItemsCollection[numInContainer];

        public ItemsCollection ItemsCollection;
        public bool Highlighted => _highlightImage.activeSelf;
        public bool Selected => _selectImage.activeSelf;
        public int numInContainer => transform.parent.GetSiblingIndex();
        public object info;

        public bool AbleToPickUp()
        {
            return true;
        }
        public void OnStartDrag() {  }
        public void OnEndDrag(bool success) {   }
        public RectTransform GetDraggedRect() => null;
        public bool AbleToDrag() => true;
        public virtual bool AbleToLanding(RectTransform r, object info) => true;
        public void OnLanding(RectTransform r, object info) { }
        public int GetHierarchyIndex() => transform.parent.GetSiblingIndex();
        public RectTransform GetRect()
        {
            return ItemImage.GetComponent<RectTransform>();
        }
        public object GetInfo()
        {
            return (ItemEntry, numInContainer);
        }
        public void OnPointerDown(PointerEventData e)
        {
            onUIEvent?.Invoke(new() {
                slot = this,
                slotInfo = info,
                shiftPressed = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift),
                mouseDown = true,
                pointerEventData = e
            });
        }
        
         
        
        public void Set(Sprite sprite)
        {
            ItemImage.sprite = sprite;
        }
        public void Highlight(bool v)  
        {
            _highlightImage?.SetActive(v);
        }
        public void DisplaySelectFrame(bool v)
        {
            _selectImage?.SetActive(v);
        }
    }

}
