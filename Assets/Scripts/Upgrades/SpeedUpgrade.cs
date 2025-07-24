using System.Collections;
using UnityEngine;
[CreateAssetMenu(menuName = "Upgrades/Speed", fileName = "Speed")]

public class SpeedUpgrade : Upgrade
    {
    public float speedMult;
    protected override void OnActivate(UpgradesController controller)
    {
        controller.GetComponent<CharacterFacade>().GetComponentInParent<PlayerMovement>().moveSpeed *= speedMult;
    }
    protected override void OnDeactivate(UpgradesController controller)
    {
        controller.GetComponent<CharacterFacade>().GetComponentInParent<PlayerMovement>().moveSpeed /= speedMult;
    }
}
