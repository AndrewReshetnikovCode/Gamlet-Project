using System.Collections;
using UnityEngine;
[CreateAssetMenu(menuName = "Upgrades/Speed", fileName = "Speed")]

public class SpeedUpgrade : Upgrade
    {
    public float speedMult;
    protected override void OnActivate(UpgradeController controller)
    {
        controller.GetComponent<CharacterFacade>().GetComponentInParent<PlayerMovement>().moveSpeed *= speedMult;
    }
    protected override void OnDeactivate(UpgradeController controller)
    {
        controller.GetComponent<CharacterFacade>().GetComponentInParent<PlayerMovement>().moveSpeed /= speedMult;
    }
}
