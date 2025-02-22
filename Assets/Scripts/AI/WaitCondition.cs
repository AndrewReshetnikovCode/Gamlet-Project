using UnityEngine;
using UnityHFSM;
[CreateAssetMenu(menuName ="AI/Condition/Wait")]
public class WaitCondition : AICondition
{
    [SerializeField] float _minRandomTime = 1;
    [SerializeField] float _maxRandomTime = 10;
    [SerializeField] bool _useRandomTime = true;

    float _reqiredTime;
    float _elapsedTime = 0;
    float _lastCallTime = 0;
    bool _firstCall = true;
    protected override void OnInit()
    {
        t.onTransition += () =>
        {
            _elapsedTime = 0; 
            _firstCall = true; 
            if (_useRandomTime)
            {
                _reqiredTime = Random.Range(_minRandomTime, _maxRandomTime);
            } 
        };

    }
    public override bool Try(Transition<StateT> s)
    {
        if (_firstCall)
        {
            _lastCallTime = Time.time;
            _firstCall = false;
        }
        _elapsedTime += Time.time - _lastCallTime;
        _lastCallTime = Time.time;
        return _elapsedTime > _reqiredTime;
    }
}
