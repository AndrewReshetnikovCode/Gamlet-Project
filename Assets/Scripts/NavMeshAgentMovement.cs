using DemiurgEngine.StatSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class NavMeshAgentMovement : MonoBehaviour
{
    public UnityEvent OnSprintApplied;
    public UnityEvent OnSprintReleased;

    bool _sprintAppliedThisFrame = false;
    bool _sprintAppliedPrevFrame = false;

    [SerializeField] NavMeshAgent _navMeshAgent;
    [SerializeField] Rigidbody _rb;
    [AutoAssignStat] public Stat _speed;
    [SerializeField] bool _updateRotation;
    [SerializeField] float _baseSpeed => _speed.BaseValue;

    public NavMeshAgent Agent => _a;

    [SerializeField] float _chargeSpeedMult;
    float _currentSpeed;
    float _forwardVelocity;
    float _rightVelocity;
    [SerializeField] float _maxSpeed;
    Vector3 _movementForce;
    NavMeshAgent _a;


    void Awake()
    {
        _a = GetComponent<NavMeshAgent>();
        _a.updateRotation = _updateRotation;
        _a.updatePosition = true;
    }
    void Update()
    {
        GetComponent<RotationController>().ApplyRotationToPoint(PlayerManager.CharacterStatic.transform.position, true);
        return;
        _movementForce = (transform.forward * _forwardVelocity + transform.right * _rightVelocity).normalized * _currentSpeed;
        if (_movementForce.magnitude > _maxSpeed)
        {
            _movementForce = _movementForce.normalized * _maxSpeed;
        }
        _rb.AddForce(_movementForce, ForceMode.Force);

        _forwardVelocity = 0;
        _rightVelocity = 0;
        if (_sprintAppliedThisFrame != _sprintAppliedPrevFrame)
        {
            if (_sprintAppliedThisFrame)
            {
                OnSprintApplied?.Invoke();
            }
            else
            {
                OnSprintReleased?.Invoke();
            }
        }
        _sprintAppliedPrevFrame = _sprintAppliedThisFrame;
        _sprintAppliedThisFrame = false;
        _currentSpeed = _baseSpeed;
    }
    public void GoTo(Vector3 pos)
    {
        _a.SetDestination(pos);
    }
    public void StopMovement()
    {
        _a.SetDestination(_a.transform.position);
    }
    [ContextMenu("Test")]
    public void GoToRandom()
    {
        Vector3 point = Random.insideUnitSphere * 30;
        point.y = 0;
        NavMeshHit hit;
        NavMesh.SamplePosition(point, out hit, 30, LayerMask.GetMask("Default"));

        GoTo(hit.position);

    }
    public void ApplyAcceleration()
    {
        _sprintAppliedThisFrame = true;
        _currentSpeed = _baseSpeed * _chargeSpeedMult;
    }
    public void ApplyMovement(float forward, float right = 0)
    {
        _rightVelocity = right;
        _forwardVelocity = forward;

    }
}
