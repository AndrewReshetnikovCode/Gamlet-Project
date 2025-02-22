using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance; // Синглтон для удобного доступа

    private float shakeDuration = 0f;   // Продолжительность тряски
    private float shakeMagnitude = 0.2f; // Интенсивность тряски
    private float dampingSpeed = 1.0f;  // Скорость затухания

    private Vector3 initialPosition;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        initialPosition = transform.localPosition;
    }

    private void Update()
    {
        if (shakeDuration > 0)
        {
            transform.localPosition = initialPosition + Random.insideUnitSphere * shakeMagnitude;

            shakeDuration -= Time.deltaTime * dampingSpeed;
        }
        else
        {
            shakeDuration = 0f;
            transform.localPosition = initialPosition;
        }
    }

    // Метод для вызова тряски
    public void TriggerShake(float duration, float magnitude)
    {
        shakeDuration = duration;
        shakeMagnitude = magnitude;
    }
}
