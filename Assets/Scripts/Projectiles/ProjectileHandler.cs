using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BulletHandler/Default")]
public class ProjectileHandler : ScriptableObject
{
    [SerializeField] float _tickCooldown = .1f;
    float _lastTick;

    protected ProjectileController _projectile;

    public virtual float Priority => 0;


    void Update()
    {
        if (Time.time - _lastTick > _tickCooldown)
        {
            _lastTick = Time.time;
            Tick();
        }
    }
    public void SetProjectile(ProjectileController projectile)
    {
        _projectile = projectile;
    }
    public virtual void OnHit(HitInfo info)
    {

    }
    protected virtual void Tick()
    {

    }
}
