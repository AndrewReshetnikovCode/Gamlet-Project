using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/DoubleJump", fileName = "DoubleJump")]
public class DoubleJump : Upgrade
{
    protected override void OnActivate(UpgradesController controller)
    {
        controller.character.GetComponent<PlayerMovement>().DoubleJump = true;
    }
    protected override void OnDeactivate(UpgradesController controller)
    {
        controller.character.GetComponent<PlayerMovement>().DoubleJump = false;
    }
}
