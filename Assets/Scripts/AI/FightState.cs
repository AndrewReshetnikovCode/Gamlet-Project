using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHFSM;

//[CreateAssetMenu(menuName ="State/FightState")]
public class FightState : AIState
{
    Vector3 _targetPos;
    Transform _targetTransform;
    CharacterRow _targetCharacter;

    float _chaseTime;
    float _tacticChangeInterval;                                                                 
    float _lastFightTacticChangeTime;
    float _chaseBreakDist;
    float _maxTimeToChase;
    int _tacticChooseRoll;

    //public override void Init()
    //{
    //    //StateMachineShortcuts.AddState(this, StateT.chase, onEnter: (s) => { _chaseTime = 0; character _movement.GoTo(_targetTransform.position); }, onLogic: (s) => { _chaseTime += Time.deltaTime; }, onExit: (s) => { _movement.StopMovement(); });
    //    StateMachineShortcuts.AddState(this, StateT.fightThink);
    //    //_fightSM.AddState(StateT.strafing, onLogic: (s) => { _movement.ApplyMovement(.2f, 1); _rotation.ApplyRotationToPoint(_targetTransform.position); });
    //    //_fightSM.AddState(StateT.fightSwimAway, onLogic: (s) => { _movement.ApplyMovement(-1); _rotation.ApplyRotationToPoint(_targetTransform.position); });

    //    AddTransitionFromAny(new Transition<StateT>(default, StateT.fightThink, (t) => Time.time - _lastFightTacticChangeTime > _tacticChangeInterval * 1, (t) => { _lastFightTacticChangeTime = Time.time; _tacticChooseRoll = Random.Range(0, 1); }));
    //    //_fightSM.AddTransition(new TransitionAfter<StateT>(StateT.chase, StateT.fightThink, _tacticChangeInterval, onTransition: (t) => _tacticChooseRoll = Random.Range(0, 3)));
    //    //_fightSM.AddTransition(new TransitionAfter<StateT>(StateT.strafing, StateT.fightThink, _tacticChangeInterval, onTransition: (t) => _tacticChooseRoll = Random.Range(0, 3)));
    //    //_fightSM.AddTransition(new Transition<StateT>(StateT.fightThink, StateT.fightSwimAway, (t) => _tacticChooseRoll == 2));
    //    AddTransition(new Transition<StateT>(StateT.fightThink, StateT.chase, (t) => _tacticChooseRoll == 0));
    //    //_fightSM.AddTransition(new Transition<StateT>(StateT.fightThink, StateT.strafing, (t) => _tacticChooseRoll == 1));

    //    SetStartState(StateT.chase);

    //    base.Init();
    //}
}
