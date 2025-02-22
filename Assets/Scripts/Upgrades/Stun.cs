using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Stun", fileName = "Stun")]
public class Stun : TemporaryUpgrade
{
    protected override void OnActivate(UpgradeController controller)
    {
        controller.character.brain.enabled = false;
        controller.character.navMeshMovement.StopMovement();
        controller.character.animator?.SetBool("Stun", true);
    }
    protected override void OnDeactivate(UpgradeController controller)
    {
        controller.character.brain.enabled = true;
        controller.character.animator?.SetBool("Stun", false);

    }
}

