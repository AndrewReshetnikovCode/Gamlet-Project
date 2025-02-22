using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class ProjectileController : MonoBehaviour
{
    public CharacterFacade character;

    Rigidbody _rb;
    [SerializeField] float _speed;
    [SerializeField] float _lifeTime;

    [SerializeField] float _damage;
    [SerializeField] float _spread = 45;

    [SerializeField] List<ProjectileHandler> _handlers;
    [SerializeField] Vector3 _dir;
    public void Init(Vector3 dir, float damage, CharacterFacade sourceCharacter)
    {
        _dir = dir;
        character = sourceCharacter;
        if (_handlers == null)
            _handlers = new();
        _rb = GetComponent<Rigidbody>();
        _damage = damage;

        dir = dir.normalized;
        dir = Quaternion.Euler(0, Random.Range(-_spread, _spread), 0) * dir;

        transform.forward = dir;
    }
    public void SetHandlers(List<ProjectileHandler> handlers)
    {
        _handlers = handlers;
        foreach (var item in _handlers)
        {
            item.SetProjectile(this);
        }
    }
    private void FixedUpdate()
    {
        if (_lifeTime < 0)
        {
            Destroy(gameObject);
        }
        _lifeTime -= Time.deltaTime;
        _rb.MovePosition(transform.position + _dir * Time.fixedDeltaTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        CharacterFacade facade;
        if (other.transform.TryGetComponent<CharacterFacade>(out facade))
        {
            facade.ApplyDamage(_damage, null);
        }
        HitInfo hit = new();
        hit.reciever = facade;
        hit.source = character;
        hit.hittedPart = other.transform;
        hit.damage = _damage;
        hit.speed = _speed;
        
        foreach (var item in _handlers)
        {
            item.OnHit(hit);
        }
        Destroy(gameObject);
    }
}
