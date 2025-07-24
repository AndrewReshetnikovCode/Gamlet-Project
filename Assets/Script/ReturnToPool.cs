using System;
using UnityEngine;

public class ReturnToPool : MonoBehaviour
{
    Action<GameObject> onDisableAction;

    public void SetPoolManager(GameObjectPool poolManager)
    {
        onDisableAction = (go) =>
        {
            if (go != null)
            {
                poolManager.Release(go);
            }
        };
    }

    void OnDisable()
    {
        onDisableAction?.Invoke(gameObject);
    }
}