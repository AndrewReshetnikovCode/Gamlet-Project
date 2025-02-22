using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform _placeholder;

    public float mouseSensitivity = 2.0f;  // Чувствительность мыши
    public float cameraDistance = 5.0f;  // Радиус камеры
    public float cameraMinDistance = 1.0f;  // Минимальная дистанция
    public float cameraMaxDistance = 10.0f;  // Максимальная дистанция
    public float rotationSmoothness = 5.0f;  // Плавность поворота камеры
    public float verticalMin = -90;
    public float verticalMax = 90;
    public LayerMask collisionMask;  // Слой для проверки столкновений

    private float currentX = 0.0f;  // Поворот по оси X
    private float currentY = 0.0f;  // Поворот по оси Y
    private float desiredDistance;  // Желаемая дистанция до самолета
    private Vector3 desiredPosition;  // Желаемая позиция камеры
    private Vector3 cameraPosition;  

    private Vector3 smoothedPosition;  // Позиция с учетом плавности
    private Vector3 forwardDir;  // Направление от самолета до камеры

    private void Start()
    {
        desiredDistance = cameraDistance;
    }

    private void Update()
    {
        // Получаем ввод от мыши
        currentX += Input.GetAxis("Mouse X") * mouseSensitivity;
        currentY -= Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Ограничиваем вертикальный поворот камеры
        currentY = Mathf.Clamp(currentY, verticalMin, verticalMax);

        // Рассчитываем направление
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        forwardDir = new Vector3(0, 0, -desiredDistance);
        desiredPosition = _placeholder.position + rotation * forwardDir;

        RaycastHit hit;
        if (Physics.Linecast(_placeholder.position, desiredPosition, out hit, collisionMask))
        {
            cameraPosition = hit.point;
        }
        else
        {
            cameraPosition = desiredPosition;
        }

        transform.position = cameraPosition;
        transform.LookAt(_placeholder);

        // Управление по оси X (наклон по оси forward)
        //float horizontalInput = Input.GetAxis("Horizontal");
        //_placeholder.Rotate(Vector3.forward, -horizontalInput * 2f);

        
    }
}

