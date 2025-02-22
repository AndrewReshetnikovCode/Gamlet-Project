using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
[CreateAssetMenu(menuName ="AI/State/ChasePlayer")]
public class ChasePlayerState : ChaseState
{
    protected override Transform GetTransform()
    {
        return PlayerManager.Instance.Player.transform;
    }
}