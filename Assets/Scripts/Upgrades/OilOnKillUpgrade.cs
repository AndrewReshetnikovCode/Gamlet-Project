using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Oil", fileName = "Oil")]

public class OilOnKillUpgrade : Upgrade
{
    [SerializeField] GameObject _sparkPrefab;
    [SerializeField] GameObject _oilPrefab;
    [SerializeField] float _samePlaceSizeMult;
    [SerializeField] float _sparkLaunchForce;
    OilHandler _h;
    protected override void OnActivate(UpgradesController controller)
    {
        OilHandler h = new();
        h.oilPrefab = _oilPrefab;
        h.onSamePlaceSizeMult = _samePlaceSizeMult;
        _h = h;
        controller.character.shooting.AddHandler(h);
    }
    protected override void OnDeactivate(UpgradesController controller)
    {
        controller.character.shooting.RemoveHandler(_h);
    }
    protected override void OnCharacterDeath(UpgradesController controller)
    {
        if (controller.HasUpgrade<IgnitionUpgrade>())
        {
            GameObject s = GameObject.Instantiate(_sparkPrefab, controller.character.GetMainTransform().position + Vector3.up, Quaternion.identity);
            Rigidbody rb = s.GetComponent<Rigidbody>();
            rb.AddForce(Quaternion.AngleAxis(Random.Range(0f, 360f), Vector3.up) * (Vector3.up + Vector3.right) * _sparkLaunchForce, ForceMode.Impulse);
        }
    }
}
