using UnityEngine;

[CreateAssetMenu(menuName = "BulletHandler/Vampirism")]

public class VampirismHandler : ProjectileHandler
{
    public float damageMult;
    public override void OnHit(HitInfo info)
    {
        if (info.reciever != null)
        {
            info.source.health.CurrentHealth += info.damage * damageMult;
        }
    }
}
