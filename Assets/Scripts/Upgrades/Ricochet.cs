using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Ricochet", fileName = "Ricochet")]

public class Ricochet : Upgrade
{
    public int quantity = 1;
    protected override void OnActivate(UpgradesController controller)
    {
        controller.character.shooting.RicochetQuantity = quantity;
    }
    protected override void OnDeactivate(UpgradesController controller)
    {
        controller.character.shooting.RicochetQuantity = 0;
    }
}
