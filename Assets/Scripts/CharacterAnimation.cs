using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class CharacterAnimation : MonoBehaviour
    {
        const string getDamageTrigger = "GetDamage";
        Animator _animator;
        void Start()
        {
            CharacterFacade character = GetComponent<CharacterFacade>();
            _animator = character.animator;
            GetComponent<CharacterFacade>().health.onDamageApplied += (d, s) => _animator.SetTrigger(getDamageTrigger);
        }
    }
}