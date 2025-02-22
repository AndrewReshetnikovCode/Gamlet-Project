using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Upgrade", fileName = "New Upgrade")]
public class Upgrade : ScriptableObject
{
    [Header("Upgrade Details")]
    public string upgradeName;
    public string description;
    public Sprite icon;

    [Header("Upgrade Progression")]
    public Upgrade strongerVersion; // ������ �� ��������� ������

    [Header("Timing")]
    public float tickInterval = 1f;

    // ������, ������� ����� ��������������
    protected virtual void OnActivate(UpgradeController controller) { }
    protected virtual void OnTick(UpgradeController controller) { }
    protected virtual void OnDeactivate(UpgradeController controller) { }

    // ������������ �������
    public void Activate(UpgradeController controller)
    {
        OnActivate(controller);
    }

    // �������� �� ������ ����������
    public void Tick(UpgradeController controller)
    {
        OnTick(controller);
    }

    // �������������� �������
    public void Deactivate(UpgradeController controller)
    {
        OnDeactivate(controller);
    }
}
