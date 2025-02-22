using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu]
public class WeaponBehaviour : ScriptableObject
{
    public Upgrade effectOnProjHit;
    public virtual void OnActivated()
    {

    }
    public virtual void OnUpdate()
    {

    }
    public virtual void OnDeactivated()
    {

    }
}

