
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

[CreateAssetMenu(menuName = "BulletHandler/SlowTime")]
public class SlowTimeHandler : ProjectileHandler
{
    [SerializeField] float _activeTime;
    [SerializeField] float _timeScale;
    public override void OnHit(HitInfo info)
    {
        if (info.reciever != null)
        {
            PlayerManager.Instance.SetTimeScale(_timeScale, true, _activeTime);
        }
    }
}
