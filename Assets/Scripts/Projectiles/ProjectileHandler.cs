using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BulletHandler/Default")]
public class ProjectileHandler : ScriptableObject
{
    float _lastTick;

    protected ProjectileController _projectile;

    public virtual float Priority => 0;

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
