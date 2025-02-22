using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class BrainContext
{
    Vector3 _p;
    public Vector3 TargetPos { get { return _p; } set { _p = value; } }
    Transform _t;
    public Transform TargetTransform { get => _t; set { _t = value; } }
    public CharacterRow _targetCharacter { get; set; }
    public List<CharacterFacade> CharsInSight { get; set; } = new();
    public Brain.TargetType TargetType { get; set; }
    public Vector3 Offset { get; set; }
}