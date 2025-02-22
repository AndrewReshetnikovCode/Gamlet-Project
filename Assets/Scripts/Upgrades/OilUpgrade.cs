using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Oil", fileName = "Oil")]

public class OilUpgrade : Upgrade
{
    [SerializeField] GameObject _oilPrefab;
    [SerializeField] float _samePlaceSizeMult;
    OilHandler _h;
    protected override void OnActivate(UpgradeController controller)
    {
        OilHandler h = new();
        h.oilPrefab = _oilPrefab;
        h.onSamePlaceSizeMult = _samePlaceSizeMult;
        _h = h;
        controller.character.shooting.AddHandler(h);
    }
    protected override void OnDeactivate(UpgradeController controller)
    {
        controller.character.shooting.RemoveHandler(_h);
    }
}
