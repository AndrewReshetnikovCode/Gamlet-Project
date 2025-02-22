using UnityEngine;

public class AimController : MonoBehaviour
{
    public Camera playerCamera;  // Камера игрока
    public float normalFOV = 60f;  // Обычное поле зрения
    public float aimFOV = 30f;  // Поле зрения при прицеливании
    public float aimSpeed = 10f;  // Скорость перехода между состояниями

    private bool isAiming = false;  // Флаг, указывающий, прицеливается ли игрок
    [SerializeField] float mouseSensMultOnAim;
    void Update()
    {
        // Проверяем нажатие правой кнопки мыши (или другой кнопки, заданной для прицеливания)
        if (Input.GetMouseButtonDown(1))
        {
            isAiming = true;  
            GetComponentInParent<PlayerCamera>().mouseSensitivity *= mouseSensMultOnAim;
        }
        else if (Input.GetMouseButtonUp(1))
        {
            isAiming = false;
            GetComponentInParent<PlayerCamera>().mouseSensitivity /= mouseSensMultOnAim;
        }

        // Плавно изменяем поле зрения камеры
        float targetFOV = isAiming ? aimFOV : normalFOV;
        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFOV, aimSpeed * Time.deltaTime);
    }
}
