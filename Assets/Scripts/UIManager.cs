using TMPro;
using UnityEngine;


public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public TMP_Text InteractionText { get => _interactionText; set => _interactionText = value; }
    public WizardHandLeftView WizardHandCounter { get => _wizardHandCounter; set => _wizardHandCounter = value; }

    [SerializeField] WizardHandLeftView _wizardHandCounter;
    [SerializeField] TMP_Text _interactionText;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {

    }
}
