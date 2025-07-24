using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Upgrades/Slowtime", fileName = "Slowtime")]

public class Slowtime : Upgrade
{
    [SerializeField] SlowTimeHandler _handler;
    protected override void OnActivate(UpgradesController controller)
    {
        controller.character.shooting.AddHandler(_handler);
    }
    protected override void OnDeactivate(UpgradesController controller)
    {
        controller.character.shooting.RemoveHandler(_handler);

    }
}
