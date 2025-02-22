using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Скорость перемещения
    public float jumpForce = 8f; // Сила прыжка
    public float gravity = -9.8f; // Гравитация
    public float slopeLimit = 45f; // Максимальный угол наклона для скольжения
    public bool DoubleJump = false; // Флаг для активации двойного прыжка

    private CharacterController characterController;
    private Vector3 velocity;
    private bool isGrounded;
    private bool canDoubleJump; // Флаг для проверки возможности второго прыжка

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        characterController.slopeLimit = slopeLimit;
    }

    void Update()
    {
        // Проверяем, на земле ли персонаж
        isGrounded = characterController.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Небольшое "прижатие" к земле
            canDoubleJump = true; // Сбрасываем возможность двойного прыжка
        }

        // Управление движением по плоскости
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        characterController.Move(move * moveSpeed * Time.deltaTime);

        // Прыжок
        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded)
            {
                velocity.y = jumpForce; // Первый прыжок
            }
            else if (DoubleJump && canDoubleJump)
            {
                velocity.y = jumpForce; // Двойной прыжок
                canDoubleJump = false; // Запрещаем второй прыжок до следующего приземления
            }
        }

        // Применение гравитации
        velocity.y += gravity * Time.deltaTime;
        if (velocity.y > 1000)
        {
            velocity.y = 1000;
        }
        characterController.Move(velocity * Time.deltaTime);

        // Удержание скольжения с наклонных поверхностей
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
