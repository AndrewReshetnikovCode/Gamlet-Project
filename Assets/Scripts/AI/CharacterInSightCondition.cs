using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityHFSM;

public class CharacterInSightCondition : AICondition
{
    public override bool Try(Transition<StateT> t)
    {
        return Brain.Context.CharsInSight.Count != 0;
    }
}