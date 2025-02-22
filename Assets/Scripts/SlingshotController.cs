using UnityEngine;

public class SlingshotController : MonoBehaviour
{
    [Header("Настройки рогатки")]
    public float maxBendAngle = 30f; // Максимальный угол сгиба
    public float releaseSpeed = 5f;  // Скорость разгибания
    public float springStiffness = 10f; // Жесткость "пружины"
    public bool isSpringEnabled = true; // Включена ли пружина
    [SerializeField] Transform _pivot;
    Vector3 _currentDir;

    private float currentBendAngle = 0f;
    private float bendVelocity = 0f;
    private bool isReleased = false;

    void Update()
    {
        if (isReleased && isSpringEnabled)
        {
            // Имитация пружины с эффектом колебания (приближение к 0 через демпфирование)
            float force = -springStiffness * currentBendAngle;
            bendVelocity += force * Time.deltaTime;
            bendVelocity *= Mathf.Exp(-releaseSpeed * Time.deltaTime); // Демпфирование
            currentBendAngle += bendVelocity * Time.deltaTime;

            // Если угол достаточно мал, останавливаем движение
            if (Mathf.Abs(currentBendAngle) < 0.1f && Mathf.Abs(bendVelocity) < 0.1f)
            {
                currentBendAngle = 0f;
                bendVelocity = 0f;
                isReleased = false;
            }
        }
        Vector3 rightAxis = Vector3.Cross(Vector3.up, _currentDir).normalized;
        // Обновление трансформа рогатки (например, поворот вокруг локальной оси X)
        //transform.RotateAround(_pivot.position, rightAxis, currentBendAngle);
        transform.localRotation = Quaternion.Euler(currentBendAngle, 0f, 0f);
    }

    public void Bend(float input)
    {
        if (!isReleased)
        {
            currentBendAngle = Mathf.Clamp(input * maxBendAngle, -maxBendAngle, maxBendAngle);
        }
    }

    public void Release()
    {
        isReleased = true;
    }

    public void StopSpring()
    {
        isSpringEnabled = false;
        bendVelocity = 0f;
    }

    public void StartSpring()
    {
        isSpringEnabled = true;
    }
    public void SetDir(Vector3 forward)
    {
        forward.y = 0;
        _currentDir = forward.normalized;
        transform.forward = _currentDir;
    }
}