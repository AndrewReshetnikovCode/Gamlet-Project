using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class upgradeDebugger : MonoBehaviour
{
    [Header("Target Controller")]
    [Tooltip("Укажите объект с UpgradeController")]
    public UpgradesController targetController;

    [Header("Upgrades")]
    [Tooltip("Список апгрейдов для добавления или удаления")]
    public Upgrade[] upgrades;

    // Добавить указанный апгрейд
    public void AddUpgrade(Upgrade upgrade)
    {
        if (targetController == null || upgrade == null)
        {
            Debug.LogWarning("TargetController или Upgrade не указаны!");
            return;
        }

        if (targetController.AddUpgrade(upgrade))
        {
            Debug.Log($"Апгрейд {upgrade.upgradeName} добавлен.");
        }
        else
        {
            Debug.LogWarning($"Не удалось добавить апгрейд {upgrade.upgradeName}.");
        }
    }

    // Удалить указанный апгрейд
    public void RemoveUpgrade(Upgrade upgrade)
    {
        if (targetController == null || upgrade == null)
        {
            Debug.LogWarning("TargetController или Upgrade не указаны!");
            return;
        }

        if (targetController.HasUpgrade(upgrade))
        {
            targetController.RemoveUpgrade(upgrade);
            Debug.Log($"Апгрейд {upgrade.upgradeName} удалён.");
        }
        else
        {
            Debug.LogWarning($"Апгрейд {upgrade.upgradeName} не найден.");
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
            EditorGUILayout.HelpBox("Пожалуйста, укажите Target Controller.", MessageType.Warning);
            return;
        }

        if (debugger.upgrades == null || debugger.upgrades.Length == 0)
        {
            EditorGUILayout.HelpBox("Добавьте апгрейды в список.", MessageType.Info);
            return;
        }

        foreach (Upgrade upgrade in debugger.upgrades)
        {
            if (upgrade == null) continue;

            EditorGUILayout.BeginHorizontal();

            // Кнопка добавления апгрейда
            if (GUILayout.Button($"Добавить {upgrade.upgradeName}", GUILayout.Height(25)))
            {
                debugger.AddUpgrade(upgrade);
            }

            // Кнопка удаления апгрейда
            if (GUILayout.Button($"Удалить {upgrade.upgradeName}", GUILayout.Height(25)))
            {
                debugger.RemoveUpgrade(upgrade);
            }

            EditorGUILayout.EndHorizontal();
        }
    }
}
#endif
