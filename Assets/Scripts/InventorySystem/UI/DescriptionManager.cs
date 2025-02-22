using System.Collections;
using UnityEngine;


public interface IDescriptionOwner
{
    string GetDescription();
}


public class DescriptionManager : MonoBehaviour
{
    [SerializeField] private Camera mainCamera; // Камера, из которой производится луч
    [SerializeField] private DescriptionUI descriptionUI; // Ссылка на UI для описания
    [SerializeField] private LayerMask interactableLayerMask; // Слои для проверки

    private void Update()
    {
        HandleRaycast();
    }

    private void HandleRaycast()
    {
        if (mainCamera == null || descriptionUI == null)
        {
            return;
        }

        
        if (UIDetectUtil.TryGetUIElementUnderCursor(out GameObject result))
        {
            IDescriptionOwner descriptionOwner = result.GetComponentInParent<IDescriptionOwner>();
            //Debug.Log(result?.name);
            if (descriptionOwner != null)
            {
                string description = descriptionOwner.GetDescription();
                descriptionUI.ShowDescription(description);
                return;
            }
        }

        descriptionUI.HideDescription();
    }
}

