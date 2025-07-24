using System.Collections.Generic;
using UnityEngine;

public static class ChildrenUtil
{
    /// <summary>
    /// Возвращает всех потомков указанного Transform в виде списка GameObject.
    /// </summary>
    public static List<GameObject> GetAsGameObjects(Transform parent)
    {
        List<GameObject> children = new List<GameObject>();
        foreach (Transform child in parent)
        {
            children.Add(child.gameObject);
        }
        return children;
    }

    /// <summary>
    /// Возвращает всех потомков указанного Transform в виде списка Transform.
    /// </summary>
    public static List<Transform> GetAsTransforms(Transform parent)
    {
        List<Transform> children = new List<Transform>();
        foreach (Transform child in parent)
        {
            children.Add(child);
        }
        return children;
    }
}