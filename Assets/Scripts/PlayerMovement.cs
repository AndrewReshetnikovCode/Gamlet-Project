using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // �������� �����������
    public float jumpForce = 8f; // ���� ������
    public float gravity = -9.8f; // ����������
    public float slopeLimit = 45f; // ������������ ���� ������� ��� ����������
    public bool DoubleJump = false; // ���� ��� ��������� �������� ������

    private CharacterController characterController;
    private Vector3 velocity;
    private bool isGrounded;
    private bool canDoubleJump; // ���� ��� �������� ����������� ������� ������

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        characterController.slopeLimit = slopeLimit;
    }

    void Update()
    {
        // ���������, �� ����� �� ��������
        isGrounded = characterController.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // ��������� "��������" � �����
            canDoubleJump = true; // ���������� ����������� �������� ������
        }

        // ���������� ��������� �� ���������
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        characterController.Move(move * moveSpeed * Time.deltaTime);

        // ������
        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded)
            {
                velocity.y = jumpForce; // ������ ������
            }
            else if (DoubleJump && canDoubleJump)
            {
                velocity.y = jumpForce; // ������� ������
                canDoubleJump = false; // ��������� ������ ������ �� ���������� �����������
            }
        }

        // ���������� ����������
        velocity.y += gravity * Time.deltaTime;
        if (velocity.y > 1000)
        {
            velocity.y = 1000;
        }
        characterController.Move(velocity * Time.deltaTime);

        // ��������� ���������� � ��������� ������������
        if (isGrounded && move == Vector3.zero)
        {
            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 1.5f))
            {
                var slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
                if (slopeAngle > slopeLimit)
                {
                    var slideDirection = Vector3.Cross(hit.normal, Vector3.up);
                    characterController.Move(slideDirection);
                }
            }
        }
    }
}
