using System.Collections.Generic;
using UnityEngine;


public class WizardHandLeftView : MonoBehaviour
{
    [SerializeField] List<GameObject> _handImages;
    [SerializeField] WizardFinger _wizardFingerData;
    void Update()
    {
        int handsLeft = _wizardFingerData.settings.maxHands - GameState.wizardHandsInstantiatedInWorld;

        for (int i = 0; i < _handImages.Count; i++)
        {
            bool displayThisIteration = i < handsLeft;
            if (i >= ShootingController.instance.CurrentWeaponState.loadedAmmo)
            {
                displayThisIteration = false;
            }
            _handImages[i].SetActive(displayThisIteration);
        }
    }
}
