using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DecisionsDatabase : ScriptableObject
{
    public List<Decision> mainDecisions;

    public List<Decision> fightDecisions;
}
