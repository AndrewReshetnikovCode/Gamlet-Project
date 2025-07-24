using UnityEngine;

public class RicochetHandler : ProjectileHandler
{
    public override float Priority => 999;
    public override void OnHit(HitInfo info)
    {
        if (info.reciever == null)
        {
            Vector3 reflected = Vector3.Reflect(info.dir, info.raycastHit.normal);
            info.source.shooting.LaunchBullet(Vector3.zero, reflected.normalized, info.weapon);
        }
    }
}
