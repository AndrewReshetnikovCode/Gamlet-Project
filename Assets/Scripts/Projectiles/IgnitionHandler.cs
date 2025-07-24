using UnityEngine;

[CreateAssetMenu(menuName = "BulletHandler/Fire")]
public class IgnitionHandler : ProjectileHandler
{
    [SerializeField] IgnitionUpgrade _ignitionUpgrade;
    [SerializeField] float _time;
    public override float Priority => -999999;
    public override void OnHit(HitInfo info)
    {
        if (info.reciever != null)
        {
            info.reciever.upgrades.ApplyTemporary(_ignitionUpgrade, _time, VisualEffects.Fire);

        }
    }
    
}
