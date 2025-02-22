using System.Collections;
using UnityEngine;


    public class PlayAnimOnDamage : MonoBehaviour
    {
        [SerializeField] Animator _a;
        [SerializeField] HealthController _h;
        // Use this for initialization
        void Start()
        {
            if (_a && _h)
            {
            _h.onDamageApplied += (d,s) => _a.SetTrigger("GetDamage");

            }
        }

    }
