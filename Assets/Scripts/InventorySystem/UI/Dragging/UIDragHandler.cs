
namespace DemiurgEngine.UI.Dragging
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using UnityEngine.EventSystems;
    using UnityEngine;
    using Unity.VisualScripting;
    using EventBus = Unity.VisualScripting.EventBus;
    using UnityEngine.UI;
    using InventorySystem;

    public class UIDragHandler : MonoBehaviour
    {
        public event Action<DragEventInfo> onDrag;
        public event Action<DragEventInfo> onStartDrag;
        public event Action<DragEventInfo> onEndDrag;

        public static UIDragHandler Instance { get; private set; }
        public bool IsDragging { get => _isDragging; set => _isDragging = value; }

        [SerializeField] float _draggedImageHeight;
        [SerializeField] float _draggedImageWidth;
        [SerializeField] Image _draggedImagePrefab;
        [SerializeField] Transform _canvasRoot;

        public bool destryImageObjectOnEnd;
        public bool makeCopyOfDraggedRect = true;

        int _slotUnderCursorNum;
         RectTransform _draggedRect;
        ItemsCollection _sourceItemsCollection;
        ItemEntry _itemEntry;
        GameObject _objectUnderCursor;
         bool _isDragging;

         Camera _uiCamera;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            _uiCamera = Camera.main; // Укажите камеру для UI, если она не Main
        }

        private void Update()
        {
            HandleInput();

            if (_isDragging && _draggedRect != null)
            {
                DragUpdate();
            }
        }
        public void SetDraggedItem(ItemEntry itemEntry)
        {
            _isDragging = true;
            _itemEntry = new() { item = itemEntry.item, quantity = itemEntry.quantity };
        }
        public void SetDraggedSprite(Sprite sprite)
        {
            _isDragging = true;

            Image image = Instantiate(_draggedImagePrefab, _canvasRoot);
            _draggedRect = image.rectTransform;
            image.sprite = sprite;
            

            _draggedRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _draggedImageWidth);
            _draggedRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _draggedImageHeight);
        }
        public void SetDraggedRect(RectTransform rect)
        {
            _isDragging = true;
            if (makeCopyOfDraggedRect)
            {
               _draggedRect = Instantiate(rect, rect.transform.root);

            }
            else
            {
                _draggedRect = rect;
            }
            _draggedRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _draggedImageWidth);
            _draggedRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _draggedImageHeight);
        }
        private void HandleInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                OnMouseClick();
            }
        }

        private void OnMouseClick()
        {
            if (_isDragging)
            {
                TryLandDraggedObject();
            }
            else
            {
                TryStartDragging();
            }
        }

        private void TryStartDragging()
        {
            if (UIDetectUtil.TryGetUIElementUnderCursor(out GameObject clickedObject) &&
                clickedObject.TryGetComponent(out IUIDraggable draggable))
            {
                _isDragging = true;


                _slotUnderCursorNum = draggable.GetHierarchyIndex();
                _objectUnderCursor = clickedObject;

                onStartDrag?.Invoke(new() { draggedRect = _draggedRect, objectUnderCursor = _objectUnderCursor, slotUnderCursorNum = _slotUnderCursorNum });
            }
        }

        private void TryLandDraggedObject()
        {
            IDragLandable landable = null;
            if (UIDetectUtil.TryGetUIElementUnderCursor(out GameObject targetObject) &&
                targetObject.TryGetComponent(out landable))
            {
                if (landable.AbleToLanding(_draggedRect, _itemEntry))
                {
                    landable.OnLanding(_draggedRect, _itemEntry);
                }
            }

            _slotUnderCursorNum = landable == null ? 0 : landable.GetHierarchyIndex();
            _isDragging = false;

            if (destryImageObjectOnEnd)
            {
                Destroy(_draggedRect.gameObject);
            }
            onEndDrag?.Invoke(new() { draggedRect = _draggedRect, itemEntry = _itemEntry, objectUnderCursor = targetObject, slotUnderCursorNum = _slotUnderCursorNum, sourceItemsCollection = _sourceItemsCollection });
        }

        private void DragUpdate()
        {
            Vector2 cursorPosition = Input.mousePosition;
            if (_draggedRect != null)
            {
                _draggedRect.position = cursorPosition;

            }

            //TriggerDragEvent();

            onDrag?.Invoke(null);
        }

        private void DisposeDragging(bool success)
        {

            _isDragging = false;
        }

        

        private void TriggerDragEvent()
        {
            Vector2 cursorPosition = Input.mousePosition;
            EventBus.Trigger(nameof(DragEventInfo), new DragEventInfo());
        }
    }
    public class DragEventInfo
    {
        public int slotUnderCursorNum;
        public ItemsCollection sourceItemsCollection;
        public ItemEntry itemEntry;
        public GameObject objectUnderCursor;
        public RectTransform draggedRect;
    }
}
