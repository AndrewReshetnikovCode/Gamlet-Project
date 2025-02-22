using UnityEngine;

public class FollowCameraRotation : MonoBehaviour
{
    public Transform cameraTransform; // Ссылка на трансформ камеры
    private void Start()
    {
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }
    }
    void Update()
    {
        if (cameraTransform != null)
        {
            // Устанавливаем поворот объекта равным повороту камеры
            transform.rotation = cameraTransform.rotation;
        }
    }
}
