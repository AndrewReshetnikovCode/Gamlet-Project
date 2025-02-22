using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Vampirism", fileName = "Vampirism")]
public class Vampirism : Upgrade
{
    [SerializeField] VampirismHandler _handler;
    protected override void OnActivate(UpgradeController controller)
    {
        controller.character.shooting.AddHandler(_handler);
    }
    protected override void OnDeactivate(UpgradeController controller)
    {
        controller.character.shooting.RemoveHandler(_handler);
    }
}
