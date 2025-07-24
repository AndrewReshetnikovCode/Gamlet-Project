using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/FireBulletsUpgrade", fileName = "FireBulletsUpgrade")]

public class FireBulletsUpgrade : Upgrade
{
    [SerializeField] IgnitionHandler _h;
    protected override void OnActivate(UpgradesController c)
    {
        c.character.shooting.AddHandler(_h);
    }
    protected override void OnDeactivate(UpgradesController c)
    {
        c.character.shooting.RemoveHandler(_h);
    }

}
