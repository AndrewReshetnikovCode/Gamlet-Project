using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Upgrade", fileName = "New Upgrade")]
public class Upgrade : ScriptableObject
{
    [Header("Upgrade Details")]
    public string upgradeName;
    public string description;
    public Sprite icon;

    [Header("Upgrade Progression")]
    public Upgrade strongerVersion; // Ссылка на усиленную версию

    [Header("Timing")]
    public float tickInterval = 1f;

    // Методы, которые можно переопределить
    protected virtual void OnActivate(UpgradeController controller) { }
    protected virtual void OnTick(UpgradeController controller) { }
    protected virtual void OnDeactivate(UpgradeController controller) { }

    // Активировать апгрейд
    public void Activate(UpgradeController controller)
    {
        OnActivate(controller);
    }

    // Вызывать на каждом обновлении
    public void Tick(UpgradeController controller)
    {
        OnTick(controller);
    }

    // Деактивировать апгрейд
    public void Deactivate(UpgradeController controller)
    {
        OnDeactivate(controller);
    }
}
