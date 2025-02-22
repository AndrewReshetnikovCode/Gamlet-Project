using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.EventSystems;
using UnityEngine;


public static class UIDetectUtil
{
    public static GameObject[] GetUIElementsAtPosition(Vector2 screenPosition)
    {
        List<RaycastResult> raycastResults = new();

        PointerEventData pointerEventData = new(EventSystem.current)
        {
            position = screenPosition
        };

        EventSystem.current.RaycastAll(pointerEventData, raycastResults);

        GameObject[] uiElements = new GameObject[raycastResults.Count];
        for (int i = 0; i < raycastResults.Count; i++)
        {
            uiElements[i] = raycastResults[i].gameObject;
        }

        return uiElements;
    }
    public static bool TryGetUIElementUnderCursor(out GameObject result)
    {
        PointerEventData pointerEventData = new(EventSystem.current)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> raycastResults = new();
        EventSystem.current.RaycastAll(pointerEventData, raycastResults);

        if (raycastResults.Count > 0)
        {
            result = raycastResults[0].gameObject;
            return true;
        }

        result = null;
        return false;
    }
}

