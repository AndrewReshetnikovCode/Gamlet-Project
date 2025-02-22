using UnityEngine;

public class AimController : MonoBehaviour
{
    public Camera playerCamera;  // ������ ������
    public float normalFOV = 60f;  // ������� ���� ������
    public float aimFOV = 30f;  // ���� ������ ��� ������������
    public float aimSpeed = 10f;  // �������� �������� ����� �����������

    private bool isAiming = false;  // ����, �����������, ������������� �� �����
    [SerializeField] float mouseSensMultOnAim;
    void Update()
    {
        // ��������� ������� ������ ������ ���� (��� ������ ������, �������� ��� ������������)
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

        // ������ �������� ���� ������ ������
        float targetFOV = isAiming ? aimFOV : normalFOV;
        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFOV, aimSpeed * Time.deltaTime);
    }
}
