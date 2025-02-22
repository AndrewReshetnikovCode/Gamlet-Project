using UnityEngine;

[DisallowMultipleComponent]
public class ColorHueRotator : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float rotationSpeed = 0.5f; // �������� �������� �������

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
            // �������� ������� ���� � ��������� ��� � HSV
            Color color = spriteRenderer.color;
            Color.RGBToHSV(color, out float h, out float s, out float v);

            // ����������� �������� H ��� �������� �����
            h += rotationSpeed * Time.deltaTime;
            if (h > 1) h -= 1; // ����� ���� ���������� ����� ���������� 1

            // ��������� ���� �� ������ ������ H
            spriteRenderer.color = Color.HSVToRGB(h, s, v);
        }
    }
}
