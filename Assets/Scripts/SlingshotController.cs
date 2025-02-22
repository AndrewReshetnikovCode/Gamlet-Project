using UnityEngine;

public class SlingshotController : MonoBehaviour
{
    [Header("��������� �������")]
    public float maxBendAngle = 30f; // ������������ ���� �����
    public float releaseSpeed = 5f;  // �������� ����������
    public float springStiffness = 10f; // ��������� "�������"
    public bool isSpringEnabled = true; // �������� �� �������
    [SerializeField] Transform _pivot;
    Vector3 _currentDir;

    private float currentBendAngle = 0f;
    private float bendVelocity = 0f;
    private bool isReleased = false;

    void Update()
    {
        if (isReleased && isSpringEnabled)
        {
            // �������� ������� � �������� ��������� (����������� � 0 ����� �������������)
            float force = -springStiffness * currentBendAngle;
            bendVelocity += force * Time.deltaTime;
            bendVelocity *= Mathf.Exp(-releaseSpeed * Time.deltaTime); // �������������
            currentBendAngle += bendVelocity * Time.deltaTime;

            // ���� ���� ���������� ���, ������������� ��������
            if (Mathf.Abs(currentBendAngle) < 0.1f && Mathf.Abs(bendVelocity) < 0.1f)
            {
                currentBendAngle = 0f;
                bendVelocity = 0f;
                isReleased = false;
            }
        }
        Vector3 rightAxis = Vector3.Cross(Vector3.up, _currentDir).normalized;
        // ���������� ���������� ������� (��������, ������� ������ ��������� ��� X)
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