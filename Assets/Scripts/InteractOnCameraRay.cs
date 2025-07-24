using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class InteractOnCameraRay : MonoBehaviour, ICameraRayReceiver
{
    public UnityEvent OnInteract;

    public bool triggerOnce = true;

    TMP_Text _interactionText;
    [SerializeField] KeyCode _interactButton = KeyCode.E;
    [SerializeField] float _maxDistance = 5f;

    bool _isInFocus;
    bool _wasTrigger = false;

    void Start()
    {
        if (_interactionText == null)
        {
            _interactionText = UIManager.instance.InteractionText;
        }
    }

    public virtual void OnRayEnter()
    {
        if (triggerOnce && _wasTrigger)
        {
            return;
        }
        if (_interactionText != null)
        {
            _interactionText.text = GetInteractionText();
        }
        _isInFocus = true;
    }
    public void OnRay()
    {
        if (Input.GetKeyDown(_interactButton))
        {
            if (triggerOnce)
            {
                if (_wasTrigger)
                {
                    return;
                }
                //Сразу очищаем надпись взаимодействия
                _interactionText.text = string.Empty;
            }
            _wasTrigger = true;
            OnInteract?.Invoke();
        }
    }

    public virtual void OnRayExit()
    {
        if (_interactionText != null)
        {
            _interactionText.text = string.Empty;
        }
        _isInFocus = false;
    }

    public virtual float GetMaxDistance()
    {
        return _maxDistance;
    }
    string GetInteractionText()
    {
        return _interactButton.ToString().ToUpper() + " - to interact";
    }
}

