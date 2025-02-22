using UnityEngine;

[DisallowMultipleComponent]
public class ColorHueRotator : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float rotationSpeed = 0.5f; // Скорость вращения оттенка

    private void Start()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
    }

    private void Update()
    {
        if (spriteRenderer != null)
        {
            // Получаем текущий цвет и переводим его в HSV
            Color color = spriteRenderer.color;
            Color.RGBToHSV(color, out float h, out float s, out float v);

            // Увеличиваем параметр H для вращения цвета
            h += rotationSpeed * Time.deltaTime;
            if (h > 1) h -= 1; // Чтобы цикл повторялся после достижения 1

            // Обновляем цвет на основе нового H
            spriteRenderer.color = Color.HSVToRGB(h, s, v);
        }
    }
}
