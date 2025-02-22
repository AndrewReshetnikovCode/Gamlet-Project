using UnityEngine;

[CreateAssetMenu(menuName = "BulletHandler/Fire")]
public class IgnitionHandler : ProjectileHandler
{
    [SerializeField] IgnitionUpgrade _ignitionUpgrade;
    public override float Priority => -999999;
    public override void OnHit(HitInfo info)
    {
        if (info.reciever != null)
        {
            info.reciever.upgrade.AddUpgrade(_ignitionUpgrade);
            if (info.reciever.upgrade.upgradesOnTickActions.ContainsKey(_ignitionUpgrade) == false)
            {
                info.reciever.upgrade.upgradesOnTickActions.Add(_ignitionUpgrade, new UpgradeController.ActionOnTick()
                {
                    source = info.source,
                    action = (a) =>
                    {
                        CharacterFacade c = a.Item3.character;
                        if (c.health.CurrentHealth == 0)
                        {
                            CheckOil(info);
                        }
                    }
                });
            }

            CheckOil(info);
        }
    }
    void CheckOil(HitInfo info)
    {
        if (info.source.upgrade.HasUpgrade<OilUpgrade>())
        {
            RaycastHit hit;
            if (Physics.Raycast(info.hittedPoint, Vector3.down, out hit, 100, LayerMask.GetMask("Oil")))
            {
                hit.transform.GetComponent<OilController>().FireUp();
            }
        }
    }
}
