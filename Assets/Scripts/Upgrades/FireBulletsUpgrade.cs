using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/FireBulletsUpgrade", fileName = "FireBulletsUpgrade")]

public class FireBulletsUpgrade : Upgrade
{
    [SerializeField] IgnitionHandler _h;
    protected override void OnActivate(UpgradeController c)
    {
        c.character.shooting.AddHandler(_h);
    }
    protected override void OnDeactivate(UpgradeController c)
    {
        c.character.shooting.RemoveHandler(_h);
    }

}
