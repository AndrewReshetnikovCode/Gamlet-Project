using UnityEngine;

[RequireComponent(typeof(SkinnedMeshRenderer))]
public class Softbody : MonoBehaviour
{
    [Header("Softbody Parameters")]
    [Tooltip("Spring stiffness coefficient")]
    [SerializeField] float _stiffness = 200f;
    [Tooltip("Damping coefficient")]
    [SerializeField] float _damping = 5f;
    [Tooltip("Mass per vertex point")]
    [SerializeField] float _mass = 1f;
    [Tooltip("Gravity acceleration magnitude")]
    [SerializeField] Vector3 _gravity = Physics.gravity;
    [Tooltip("Offset from collision surface")]
    [SerializeField] float _collisionOffset = 0.01f;
    [Tooltip("Raycast layers to collide with softbody")]
    [SerializeField] LayerMask _collisionLayers;
    [Tooltip("Factor of how strongly points push center")]
    [SerializeField] float _pointPushFactor = 1f;
    [Tooltip("Threshold for stopping small oscillations")]
    [SerializeField] float _stopThreshold = 0.01f;

    [Header("Bone Transforms")]
    [SerializeField] Transform[] _bones;

    class Point
    {
        public Vector3 _restPosition;
        public Vector3 _position;
        public Vector3 _velocity;
    }

    Point[] _points;
    Vector3 _centerPosition;
    Vector3 _centerVelocity;

    void Awake()
    {
        InitializePoints();
    }

    void InitializePoints()
    {
        _centerPosition = transform.position;
        _centerVelocity = Vector3.zero;

        _points = new Point[_bones.Length];
        for (int i = 0; i < _bones.Length; i++)
        {
            var p = new Point();
            p._restPosition = _bones[i].position;
            p._position = p._restPosition;
            p._velocity = Vector3.zero;
            _points[i] = p;
        }
    }

    void FixedUpdate()
    {
        float dt = Time.fixedDeltaTime;
        SimulatePoints(dt);
        SimulateCenter(dt);
        HandleCollisions();
        ApplyToBones();
    }

    void SimulatePoints(float dt)
    {
        for (int i = 0; i < _points.Length; i++)
        {
            Point p = _points[i];
            Vector3 springForce = -_stiffness * (p._position - p._restPosition);
            Vector3 dampForce = -_damping * p._velocity;
            Vector3 totalForce = springForce + dampForce + _gravity * _mass;
            Vector3 accel = totalForce / _mass;
            p._velocity += accel * dt;
            p._position += p._velocity * dt;

            // Stop small oscillations
            if (p._velocity.sqrMagnitude < _stopThreshold * _stopThreshold &&
                (p._position - p._restPosition).sqrMagnitude < _stopThreshold * _stopThreshold)
            {
                p._velocity = Vector3.zero;
                p._position = p._restPosition;
            }
        }
    }

    void SimulateCenter(float dt)
    {
        Vector3 forceFromPoints = Vector3.zero;
        for (int i = 0; i < _points.Length; i++)
            forceFromPoints += (_points[i]._position - _points[i]._restPosition);
        forceFromPoints *= _pointPushFactor;

        float centerMass = _mass * _points.Length;
        Vector3 totalForce = forceFromPoints + _gravity * centerMass;
        Vector3 accel = totalForce / centerMass;

        Vector3 prevCenter = _centerPosition;
        _centerVelocity += accel * dt;
        _centerPosition += _centerVelocity * dt;

        // Stop small center oscillations
        if (_centerVelocity.sqrMagnitude < _stopThreshold * _stopThreshold &&
            forceFromPoints.sqrMagnitude < _stopThreshold * _stopThreshold)
        {
            _centerVelocity = Vector3.zero;
            _centerPosition = prevCenter; // keep center at last stable
        }

        transform.position = _centerPosition;

        Vector3 delta = _centerPosition - prevCenter;
        for (int i = 0; i < _points.Length; i++)
            _points[i]._restPosition += delta;
    }

    void HandleCollisions()
    {
        Vector3 worldCenter = _centerPosition;
        for (int i = 0; i < _points.Length; i++)
        {
            Point p = _points[i];
            Vector3 dir = p._position - worldCenter;
            float distance = dir.magnitude;
            if (distance <= Mathf.Epsilon)
                continue;

            Ray ray = new Ray(worldCenter, dir.normalized);
            if (Physics.Raycast(ray, out RaycastHit hit, distance, _collisionLayers, QueryTriggerInteraction.Ignore))
            {
                Vector3 targetPos = hit.point + hit.normal * _collisionOffset;
                p._velocity = Vector3.Reflect(p._velocity, hit.normal);
                p._position = targetPos;
            }
        }
    }

    void ApplyToBones()
    {
        for (int i = 0; i < _bones.Length; i++)
        {
            Vector3 localPos = _bones[i].parent.InverseTransformPoint(_points[i]._position);
            _bones[i].localPosition = localPos;
        }
    }
}
