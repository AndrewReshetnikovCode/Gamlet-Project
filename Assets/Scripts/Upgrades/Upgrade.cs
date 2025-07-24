using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Upgrade", fileName = "New Upgrade")]
public class Upgrade : ScriptableObject
{
    [Header("Upgrade Details")]
    public string upgradeName;
    public string description;
    public Sprite icon;

    [Header("Upgrade Progression")]
    public Upgrade strongerVersion;

    [Header("Timing")]
    public float tickInterval = 1f;

    protected virtual void OnActivate(UpgradesController controller) { }
    protected virtual void OnTick(UpgradesController controller) { }
    protected virtual void OnDeactivate(UpgradesController controller) { }
    protected virtual void OnCharacterDeath(UpgradesController controller) { }

    public void Activate(UpgradesController controller)
    {
        OnActivate(controller);
    }

    public void Tick(UpgradesController controller)
    {
        OnTick(controller);
    }

    public void Deactivate(UpgradesController controller)
    {
        OnDeactivate(controller);
    }
}
