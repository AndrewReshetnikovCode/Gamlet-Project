using UnityEngine;

public class DragObject : MonoBehaviour
{
    [SerializeField] float _handleScaleOnHover;

    private Camera mainCamera;
    private bool isDragging = false;
    private Vector3 offset;

    void Start()
    {
        mainCamera = Camera.main;
    }

    private void OnMouseEnter()
    {
        transform.localScale = transform.localScale * _handleScaleOnHover;
    }
    private void OnMouseExit()
    {
        transform.localScale = transform.localScale / _handleScaleOnHover;
    }
    void OnMouseDown()
    {
        isDragging = true;
        Vector3 mousePosition = GetMouseWorldPosition();
        offset = transform.position - mousePosition;
    }

    void OnMouseUp()
    {
        isDragging = false;
    }

    void Update()
    {
        if (isDragging)
        {
            Vector3 mousePosition = GetMouseWorldPosition();
            transform.position = new Vector3(mousePosition.x + offset.x, mousePosition.y + offset.y, transform.position.z);
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mouseScreenPosition = Input.mousePosition;
        mouseScreenPosition.z = Mathf.Abs(mainCamera.transform.position.z - transform.position.z);
        return mainCamera.ScreenToWorldPoint(mouseScreenPosition);
    }
}