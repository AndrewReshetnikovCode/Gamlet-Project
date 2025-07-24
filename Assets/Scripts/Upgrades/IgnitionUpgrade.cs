using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Ignition", fileName = "Ignition")]

public class IgnitionUpgrade : Upgrade
{
    public float damage;
    protected override void OnActivate(UpgradesController controller)
    {

    }
    protected override void OnTick(UpgradesController controller)
    {
        base.OnTick(controller);
        controller.character.ApplyDamage(damage, this);
        
    }
    protected override void OnDeactivate(UpgradesController controller)
    {
        
    }
}
