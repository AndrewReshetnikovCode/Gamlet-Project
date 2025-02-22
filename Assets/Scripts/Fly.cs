using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fly : MonoBehaviour
{
    [SerializeField] Transform _base;
    [SerializeField] float _min;
    [SerializeField] float _max;
    [SerializeField] float _heightChangeSpeed;
    Vector3 _currentTarget;
    bool up = false;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 t = _base.position;
        t.y += _min;
        _currentTarget = t;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = Vector3.up * _min + Vector3.up * (Mathf.Sin(Time.time * _heightChangeSpeed) + 1) * (_max - _min); //Vector3.Lerp(transform.position, _currentTarget, _heightChangeSpeed);
        //if (Vector3.Distance(transform.position, _currentTarget) < 0.01f)
        //{
        //    Vector3 t = _base.position;

        //    if (up)
        //    {
        //        t.y += _min;
        //    }
        //    else
        //    {
        //        t.y += _max;
        //    }

        //    _currentTarget = t;
        //    up = !up;
        //}
    }
}
