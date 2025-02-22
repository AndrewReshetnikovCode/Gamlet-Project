using UnityEngine;

public class TextVisibilityController : MonoBehaviour
{
    public Camera mainCamera; // Камера, которая смотрит на текст
    public RectTransform textRect; // RectTransform текста на Canvas
    public float minScreenSize = 50f; // Минимальный размер текста на экране для отображения

    private CanvasRenderer canvasRenderer;

    void Start()
    {
        canvasRenderer = textRect.GetComponent<CanvasRenderer>();
        if (mainCamera == null)
        {
            mainCamera = Camera.main; 
        }
        Vector3[] worldCorners = new Vector3[4];
        textRect.GetWorldCorners(worldCorners);

        // Преобразуем углы текста в экранные координаты
        Vector3 screenCorner1 = mainCamera.WorldToScreenPoint(worldCorners[0]);
        Vector3 screenCorner2 = mainCamera.WorldToScreenPoint(worldCorners[2]);

        // Рассчитываем размер текста в пикселях на экране
        float screenWidth = Mathf.Abs(screenCorner2.x - screenCorner1.x);
        float screenHeight = Mathf.Abs(screenCorner2.y - screenCorner1.y);

        // Если размер текста меньше минимального порога, скрываем его
        bool isVisible = screenWidth >= minScreenSize && screenHeight >= minScreenSize;

        // Устанавливаем видимость текста
        canvasRenderer.SetAlpha(isVisible ? 1f : 0f);
    }

}