using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/TrueSight", fileName = "TrueSight")]

public class TrueSightUpgrade : Upgrade
{
    protected override void OnActivate(UpgradeController controller)
    {
        PlayerManager.Instance.TrueSightActive = true;
    }
    protected override void OnDeactivate(UpgradeController controller)
    {
        PlayerManager.Instance.TrueSightActive = false;
    }
}
