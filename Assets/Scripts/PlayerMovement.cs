using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    const float MAX_FALL_SPEED = 1000f;

    public float moveSpeed = 5f;
    public float jumpForce = 8f;
    public float gravity = -9.8f;
    public float slopeLimit = 45f;
    public bool DoubleJump = false;

    [Header("OnGetDamage")]
    public bool slow;
    public float duration;
    public float percent;

    float _baseSpeed;
    CharacterController _characterController;
    Vector3 _velocity;
    Vector3 _inputMove;
    bool _inputJump;
    bool _isGrounded;
    bool _canDoubleJump;

    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _characterController.slopeLimit = slopeLimit;

        _baseSpeed = moveSpeed;

        GetComponent<CharacterFacade>().health.onDamageApplied += (v, s) => SetSlow();
    }

    void Update()
    {
        HandleInput();

        _isGrounded = _characterController.isGrounded;

        if (_isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f; // Небольшое "прижатие" к земле
            _canDoubleJump = true;
        }

        _characterController.Move(_inputMove * moveSpeed * Time.deltaTime);

        if (_inputJump)
        {
            if (_isGrounded)
            {
                _velocity.y = jumpForce; // Первый прыжок
            }
            else if (DoubleJump && _canDoubleJump)
            {
                _velocity.y = jumpForce; // Двойной прыжок
                _canDoubleJump = false; // Запрещаем второй прыжок до следующего приземления
            }
        }

        //Применение гравитации
        _velocity.y += gravity * Time.deltaTime;
        if (_velocity.y > MAX_FALL_SPEED)
        {
            _velocity.y = MAX_FALL_SPEED;
        }
        _characterController.Move(_velocity * Time.deltaTime);

        //Скольжение
        if (_isGrounded && _inputMove == Vector3.zero)
        {
            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 1.5f))
            {
                var slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
                if (slopeAngle > slopeLimit)
                {
                    var slideDirection = Vector3.Cross(hit.normal, Vector3.up);
                    _characterController.Move(slideDirection);
                }
            }
        }
    }
    void HandleInput()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        _inputMove = transform.right * moveX + transform.forward * moveZ;
        _inputMove = _inputMove.normalized;

        _inputJump = Input.GetButtonDown("Jump");
    }
    void SetSlow()
    {
        if (slow)
        {
            StopAllCoroutines();
            ResetSpeed();
            StartCoroutine(InterpolateSpeed());
        }
    }
    void ResetSpeed()
    {
        moveSpeed = _baseSpeed;
    }
    IEnumerator InterpolateSpeed()
    {
        float elapsed = 0;
        float slowedSpeed = moveSpeed * percent;
        float normalSpeed = moveSpeed;


        while (elapsed < duration)
        {
            moveSpeed = Mathf.Lerp(slowedSpeed, normalSpeed, elapsed / duration);

            elapsed += Time.deltaTime;

            yield return null;
        }
    }
}
