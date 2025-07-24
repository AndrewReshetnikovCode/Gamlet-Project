using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Stun", fileName = "Stun")]
public class Stun : Upgrade
{
    protected override void OnActivate(UpgradesController controller)
    {
        controller.character.brain.enabled = false;
        controller.character.navMeshMovement.StopMovement();
        controller.character.animator?.SetBool("Stun", true);
    }
    protected override void OnDeactivate(UpgradesController controller)
    {
        controller.character.brain.enabled = true;
        controller.character.animator?.SetBool("Stun", false);
    }
}

