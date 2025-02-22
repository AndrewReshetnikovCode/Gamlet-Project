using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance; // �������� ��� �������� �������

    private float shakeDuration = 0f;   // ����������������� ������
    private float shakeMagnitude = 0.2f; // ������������� ������
    private float dampingSpeed = 1.0f;  // �������� ���������

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

    // ����� ��� ������ ������
    public void TriggerShake(float duration, float magnitude)
    {
        shakeDuration = duration;
        shakeMagnitude = magnitude;
    }
}
