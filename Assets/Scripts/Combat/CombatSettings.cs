using UnityEngine;

[CreateAssetMenu]
public class CombatSettings : ScriptableObject
{
    public float attackRaycastLength;
    //euler angle
    public Vector3 jumpRaycastDirOffset;
}
