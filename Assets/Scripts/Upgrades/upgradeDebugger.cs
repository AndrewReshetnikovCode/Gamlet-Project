using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class upgradeDebugger : MonoBehaviour
{
    [Header("Target Controller")]
    [Tooltip("������� ������ � UpgradeController")]
    public UpgradesController targetController;

    [Header("Upgrades")]
    [Tooltip("������ ��������� ��� ���������� ��� ��������")]
    public Upgrade[] upgrades;

    // �������� ��������� �������
    public void AddUpgrade(Upgrade upgrade)
    {
        if (targetController == null || upgrade == null)
        {
            Debug.LogWarning("TargetController ��� Upgrade �� �������!");
            return;
        }

        if (targetController.AddUpgrade(upgrade))
        {
            Debug.Log($"������� {upgrade.upgradeName} ��������.");
        }
        else
        {
            Debug.LogWarning($"�� ������� �������� ������� {upgrade.upgradeName}.");
        }
    }

    // ������� ��������� �������
    public void RemoveUpgrade(Upgrade upgrade)
    {
        if (targetController == null || upgrade == null)
        {
            Debug.LogWarning("TargetController ��� Upgrade �� �������!");
            return;
        }

        if (targetController.HasUpgrade(upgrade))
        {
            targetController.RemoveUpgrade(upgrade);
            Debug.Log($"������� {upgrade.upgradeName} �����.");
        }
        else
        {
            Debug.LogWarning($"������� {upgrade.upgradeName} �� ������.");
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(upgradeDebugger))]
public class UpgradeDebuggerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        upgradeDebugger debugger = (upgradeDebugger)target;

        if (debugger.targetController == null)
        {
            EditorGUILayout.HelpBox("����������, ������� Target Controller.", MessageType.Warning);
            return;
        }

        if (debugger.upgrades == null || debugger.upgrades.Length == 0)
        {
            EditorGUILayout.HelpBox("�������� �������� � ������.", MessageType.Info);
            return;
        }

        foreach (Upgrade upgrade in debugger.upgrades)
        {
            if (upgrade == null) continue;

            EditorGUILayout.BeginHorizontal();

            // ������ ���������� ��������
            if (GUILayout.Button($"�������� {upgrade.upgradeName}", GUILayout.Height(25)))
            {
                debugger.AddUpgrade(upgrade);
            }

            // ������ �������� ��������
            if (GUILayout.Button($"������� {upgrade.upgradeName}", GUILayout.Height(25)))
            {
                debugger.RemoveUpgrade(upgrade);
            }

            EditorGUILayout.EndHorizontal();
        }
    }
}
#endif
