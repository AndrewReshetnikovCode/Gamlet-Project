using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/TrueSight", fileName = "TrueSight")]

public class TrueSightUpgrade : Upgrade
{
    protected override void OnActivate(UpgradesController controller)
    {
        PlayerManager.Instance.TrueSightActive = true;
    }
    protected override void OnDeactivate(UpgradesController controller)
    {
        PlayerManager.Instance.TrueSightActive = false;
    }
}
