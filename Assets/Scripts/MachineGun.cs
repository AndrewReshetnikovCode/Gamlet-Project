using UnityEngine;

public class MachineGun : MonoBehaviour
{
    [SerializeField] ProjectileController _projectilePrefab;
    [SerializeField] float _interval = .1f;
    [SerializeField] float _bulletSpeed = 10f;
    [SerializeField] float _damage = 1;
    [SerializeField] float _radius = 10;
    [SerializeField] LayerMask _layerMask;
    Transform _target;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(nameof(Shoot), 0, _interval);
    }
    private void Update()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, _radius, _layerMask);

        if (hits.Length != 0)
        {
            if (_target != null)
            {
                transform.LookAt(_target);
                return;
            }
            _target = hits[Random.Range(0, hits.Length)].transform;
        }
        else
        {
            _target = null;
        }

    }
    void Shoot()
    {
        if (_target != null)
        {
            ProjectileController p = Instantiate(_projectilePrefab, transform.position, Quaternion.identity);
            p.Init(transform.forward * _bulletSpeed, _damage, null);
        }
    }
}
