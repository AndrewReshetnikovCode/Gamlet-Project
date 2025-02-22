using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraderController : MonoBehaviour, ICameraRayReceiver
{
    [SerializeField] private GameObject crosshair;

    [SerializeField] private GameObject interactionPrompt;
    [SerializeField] private GameObject targetObject;
    [SerializeField] private KeyCode interactButton = KeyCode.E;
    [SerializeField] private float maxDistance = 5f;

    private bool isInFocus;

    void Awake()
    {
        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(false);
        }
    }

    public void OnRayEnter()
    {
        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(true);
            crosshair?.SetActive(false);
        }
        isInFocus = true;
    }

    public void OnRay()
    {
        if (isInFocus && Input.GetKeyDown(interactButton) && targetObject != null)
        {
            targetObject.SetActive(true);
            PlayerManager.Instance.CursorLocked = false;
        }
    }

    public void OnRayExit()
    {
        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(false);
            crosshair?.SetActive(true);
        }
        isInFocus = false;
    }

    public float GetMaxDistance()
    {
        return maxDistance;
    }
}

