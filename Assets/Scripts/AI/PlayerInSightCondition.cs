using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityHFSM;

public class PlayerInSightCondition : AICondition
{
    Transform _player;
    protected override void OnInit()
    {
        _player = PlayerManager.Instance.Player.transform;
    }
    public override bool Try(Transition<StateT> t)
    {
        return Vector3.Distance(_player.position, Brain.transform.position) < Brain.LookDist;
    }
}