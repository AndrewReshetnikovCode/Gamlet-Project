using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CloneController : MonoBehaviour
{
    public const string spawnTrigger = "spawn";
    public const string destroyTrigger = "destroy";

    public CharacterFacade character;

    public float samplePosMaxDistance;
    public LayerMask groundLayer;

    public CharacterFacade original;
    public Animator animator;

    public float destroyDelay;
}
