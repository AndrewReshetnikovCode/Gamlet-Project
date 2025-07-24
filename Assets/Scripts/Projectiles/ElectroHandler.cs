using UnityEngine;

[CreateAssetMenu(menuName = "BulletHandler/Electricity")]
public class ElectroHandler : ProjectileHandler
{
    [SerializeField] Stun _stun;
    public override void OnHit(HitInfo info)
    {
        if (info.reciever != null)
        {
            info.reciever.upgrades.AddUpgrade(_stun);
        }
    }
}

