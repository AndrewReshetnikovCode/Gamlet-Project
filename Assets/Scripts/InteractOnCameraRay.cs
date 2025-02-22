using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts
{
    public class InteractOnCameraRay : MonoBehaviour, ICameraRayReceiver
    {
        public UnityEvent OnInteract;

        public bool triggerOnce = true;
        public bool unlockCursor;

        [SerializeField] private GameObject interactionPrompt;
        [SerializeField] private GameObject activasionTarget;
        [SerializeField] private KeyCode interactButton = KeyCode.E;
        [SerializeField] private float maxDistance = 5f;

        private bool isInFocus;
        bool _wasTrigger = false;
        void Awake()
        {
            if (interactionPrompt != null)
            {
                interactionPrompt.SetActive(false);
            }
        }

        public virtual void OnRayEnter()
        {
            if (triggerOnce && _wasTrigger)
            {
                return;
            }
            if (interactionPrompt != null)
            {
                interactionPrompt.SetActive(true);
            }
            isInFocus = true;
        }

        public virtual void OnRay()
        {
            if (triggerOnce && _wasTrigger)
            {
                return;
            }
            if (isInFocus && Input.GetKeyDown(interactButton))
            {
                if (activasionTarget != null)
                {
                    activasionTarget.SetActive(true);
                }
                if (unlockCursor)
                {
                    PlayerManager.Instance.CursorLocked = false;
                }
                _wasTrigger = true;
                OnInteract.Invoke();
                
            }
        }

        public virtual void OnRayExit()
        {
            if (interactionPrompt != null)
            {
                interactionPrompt.SetActive(false);
            }
            isInFocus = false;
        }

        public virtual float GetMaxDistance()
        {
            return maxDistance;
        }
    }
}
