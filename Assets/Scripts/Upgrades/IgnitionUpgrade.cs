using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Ignition", fileName = "Ignition")]

public class IgnitionUpgrade : TemporaryUpgrade
{
    public float damage;
    protected override void OnActivate(UpgradeController controller)
    {
        controller.character.visualEffect.ActivateBurn(true);
    }
    protected override void OnTick(UpgradeController controller)
    {
        base.OnTick(controller);
        controller.character.ApplyDamage(damage, this);
        
    }
    protected override void OnDeactivate(UpgradeController controller)
    {
        controller.character.visualEffect.ActivateBurn(false);
        //HACK
        controller.character.upgrade.upgradesOnTickActions.Remove(this);
    }
}
