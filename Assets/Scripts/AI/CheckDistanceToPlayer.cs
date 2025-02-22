using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Condition/CheckDistanceToPlayer")]
public class CheckDistanceToPlayer : CheckDistanceToTransform
{
    Transform _player;
    protected override void OnInit()
    {
        base.OnInit();
        _player = PlayerManager.Instance.Player.transform;
    }
    protected override Transform GetTargetTransform()
    {
        return _player;
    }
}