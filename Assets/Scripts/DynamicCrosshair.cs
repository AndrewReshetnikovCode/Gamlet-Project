using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class DynamicCrosshair : MonoBehaviour
    {
        [SerializeField] float defaultSize = 40;
        [SerializeField] RectTransform _rectTransform;
        [SerializeField] RecoilEffect _recoil;
        [SerializeField] float _mult = 1;
        // Update is called once per frame
        void Update()
        {
            _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, defaultSize + _recoil.currentSpread * _mult);
            _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, defaultSize + _recoil.currentSpread * _mult);
        }
    }
}