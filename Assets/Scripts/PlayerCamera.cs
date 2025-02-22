using System.Collections;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Transform playerBody;     // ������ �� ������ ���������
    public Vector3 offset = new Vector3(0f, 1.5f, 0f); // �������� ������ ������������ ���������
    public float mouseSensitivity = 100f;   // ���������������� ����
    public float verticalLookLimit = 90f;   // ����������� ������������� ������ (� ��������)

    private float xRotation = 0f;    // ������� ���� �������� �� ��� X (�����-����)

    void Start()
    {
        // �������� ������ ���� � ��������� ��� � ������ ������
        Cursor.lockState = CursorLockMode.Locked;
        StartCoroutine(WaitOneFrame());
        enabled = false;
    }
    IEnumerator WaitOneFrame() { yield return new WaitForEndOfFrame(); enabled = true; }

    void Update()
    {
        // �������� �������� ����
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // ������������ �������� ������ ����� � ����
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -verticalLookLimit, verticalLookLimit);

        // ��������� �������� �����/���� � ������ (�� ��� X)
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // ������� ��������� �����/������ (�� ��� Y)
        playerBody.Rotate(Vector3.up * mouseX);

        // ��������� ������� ������ � ������ ��������
        transform.position = playerBody.position + offset;

        // ��������� �������� ������ �� ����������� ������ � �������
        transform.rotation = playerBody.rotation * Quaternion.Euler(xRotation, 0f, 0f);
    }
}
