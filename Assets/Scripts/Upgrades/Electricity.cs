using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Electricity", fileName = "Electricity")]
public class Electricity : Upgrade
{
    [SerializeField] ElectroHandler _bulletHandler;
    protected override void OnActivate(UpgradeController controller)
    {
        controller.character.shooting.AddHandler(_bulletHandler);
    }
    protected override void OnDeactivate(UpgradeController controller)
    {
        controller.character.shooting.RemoveHandler(_bulletHandler);

    }
}

