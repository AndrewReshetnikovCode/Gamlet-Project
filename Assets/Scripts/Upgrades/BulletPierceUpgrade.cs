using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Pierce", fileName = "Pierce")]
public class BulletPierceUpgrade : Upgrade
{
    [SerializeField] int maxCount;
    protected override void OnActivate(UpgradeController controller)
    {
        controller.character.shooting.PierceQuantity = maxCount;
    }
    protected override void OnDeactivate(UpgradeController controller)
    {
        controller.character.shooting.PierceQuantity = 0;
    }
}
