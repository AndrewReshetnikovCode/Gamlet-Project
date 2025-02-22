using DemiurgEngine.StatSystem;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityHFSM;

public class Brain : MonoBehaviour
{
    public const string flutteringAnimTrigger = "GetDamage";
    enum Behaviour
    {
        chase,
        fleeWhenClose,
        flee
    }
    public enum TargetType
    {
        noTarget,    
        enemy,
        danger,
        potentionalFriend,
        foodSource,
        grass,
        meat,
        worldPos,
        localPos
    }

    public BrainContext Context { get; private set; }
    public CharacterFacade Character => _character;

    public float LookDist => _lookDist;

    CharacterFacade _character;
    [SerializeField] StatesCollection _statesCollection;
    [SerializeField] Transform _raycastOrigin;
    [SerializeField] Animator _animator;
    [SerializeField] NavMeshAgentMovement _movement;
    [SerializeField] RotationController _rotation;


    [SerializeField] float _lookDist;
    [SerializeField] float _lookInterval;
    [SerializeField] LayerMask _targetLayer;
    [SerializeField] LayerMask _groundLayer;

    [Tooltip("На каком расстоянии персонаж начинает преследовать цель")]
    [SerializeField] float _chaseStartDist;
    [Tooltip("На каком расстоянии персонаж перестает преследовать цель")]
    
    [SerializeField] float _healthPercentToFlee;

    [SerializeField] float _stoppingDist = 0.5f;
    [SerializeField] float _roamRadius = 200;
    
    StateMachine<StateT> _baseSM = new();
    StateMachine<StateT> _fightSM = new();
    [AutoAssignStat] Stat _health;
    
    public void Init()
    {
        Context = new BrainContext();
        Context.TargetTransform = GameObject.FindGameObjectWithTag("Player").transform;
        _character = GetComponent<CharacterFacade>();

        //_fightSM.AddState(StateT.chase, onEnter: (s) => { _chaseTime = 0; _movement.GoTo(_targetTransform.position); }, onLogic: (s) => { _chaseTime += Time.deltaTime; }, onExit: (s) => { _movement.StopMovement(); });
        //_fightSM.AddState(StateT.fightThink);
        ////_fightSM.AddState(StateT.strafing, onLogic: (s) => { _movement.ApplyMovement(.2f, 1); _rotation.ApplyRotationToPoint(_targetTransform.position); });
        ////_fightSM.AddState(StateT.fightSwimAway, onLogic: (s) => { _movement.ApplyMovement(-1); _rotation.ApplyRotationToPoint(_targetTransform.position); });

        //_fightSM.AddTransitionFromAny(new Transition<StateT>(default, StateT.fightThink, (t) => Time.time - _lastFightTacticChangeTime > _tacticChangeInterval * 1, (t) => { _lastFightTacticChangeTime = Time.time; _tacticChooseRoll = Random.Range(0, 1); }));
        ////_fightSM.AddTransition(new Transition<StateT>(StateT.boost, StateT.chase));
        ////_fightSM.AddTransition(new TransitionAfter<StateT>(StateT.chase, StateT.fightThink, _tacticChangeInterval, onTransition: (t) => _tacticChooseRoll = Random.Range(0, 3)));
        ////_fightSM.AddTransition(new TransitionAfter<StateT>(StateT.strafing, StateT.fightThink, _tacticChangeInterval, onTransition: (t) => _tacticChooseRoll = Random.Range(0, 3)));
        ////_fightSM.AddTransition(new Transition<StateT>(StateT.fightThink, StateT.fightSwimAway, (t) => _tacticChooseRoll == 2));
        //_fightSM.AddTransition(new Transition<StateT>(StateT.fightThink, StateT.chase, (t) => _tacticChooseRoll == 0));
        ////_fightSM.AddTransition(new Transition<StateT>(StateT.fightThink, StateT.strafing, (t) => _tacticChooseRoll == 1));

        //_fightSM.SetStartState(StateT.chase);

        //_baseSM.AddState(StateT.idle);
        //_baseSM.AddState(StateT.think, onEnter: (s) => { UpdateTarget(); });
        //_baseSM.AddState(StateT.moveToPos, onEnter: (s) => { _movement.GoTo(_targetPos); }, onExit: (s) => { _movement.StopMovement(); });
        ////_baseSM.AddState(StateT.moveToFood, onLogic: (s) => { _movement.ApplyMovement(1); _rotation.ApplyRotationToPoint(_targetTransform.position); });
        ////_baseSM.AddState(StateT.fluttering, onLogic: (s) => { _animator.SetTrigger(flutteringAnimTrigger); UpdateRandomRotationTargetOnFluttering(); _rotation.ApplyRotationToPoint(_targetPos); });
        //_baseSM.AddState(StateT.fight, _fightSM);
        ////_baseSM.AddState(StateT.flee, onLogic: (s) => { if (WantToBoost()) _bc.TryToApplyBoost(); _movement.ApplyMovement(1); _rotation.ApplyRotationToDirection(Quaternion.LookRotation((transform.position - _targetTransform.position).normalized)); });

        //_baseSM.AddTransition(new TransitionAfter<StateT>(StateT.idle, StateT.think, _tacticChangeInterval + Random.Range(-0.5f, 0.5f)));

        //_baseSM.AddTransition(new Transition<StateT>(StateT.think, StateT.moveToPos, (t) => _currentTargetType == TargetType.worldPos));
        //_baseSM.AddTransition(new Transition<StateT>(StateT.think, StateT.fight, (t) => _currentTargetType == TargetType.enemy, (t) => EventBus.Trigger("Aggro",new AggroEvent() { character = _character, target = _targetTransform })));
        //_baseSM.AddTransition(new Transition<StateT>(StateT.think, StateT.flee, (t) => _currentTargetType == TargetType.danger));

        //_baseSM.AddTransition(new Transition<StateT>(StateT.fight, StateT.idle, (t) => _targetTransform == null || (_targetTransform != null && Vector3.Distance(transform.position, _targetTransform.position) > _chaseBreakDist)));
        //_baseSM.AddTransition(new Transition<StateT>(StateT.fight, StateT.flee, (t) => WantToFlee()));
        //_baseSM.AddTransition(new Transition<StateT>(StateT.flee, StateT.idle, (t) => _targetTransform == null || (_targetTransform != null && Vector3.Distance(transform.position, _targetTransform.position) > _chaseBreakDist)));

        //_baseSM.AddTransition(new Transition<StateT>(StateT.moveToPos, StateT.idle, (t) => Vector3.Distance(transform.position, _targetPos) < _stoppingDist));
        //_baseSM.AddTransition(new Transition<StateT>(StateT.moveToFood, StateT.idle, (t) => _targetTransform == null));

        //_baseSM.AddTriggerTransitionFromAny("Pierce", new Transition<StateT>(default, StateT.fluttering));
        //_baseSM.AddTriggerTransitionFromAny("PierceReleased", new Transition<StateT>(default, StateT.idle));
        //_baseSM.AddTriggerTransitionFromAny("Attacked", new Transition<StateT>(default, StateT.fight));

        _baseSM = _statesCollection.GetStateMachine();

        _baseSM.SetStartState(StateT.idle);
        _baseSM.Init();

        InvokeRepeating(nameof(Look), 1, _lookInterval);




    }
    
    void Update()
    {
        _baseSM.OnLogic();
        Debug.Log(_baseSM.GetActiveHierarchyPath());
    }
    
    public void OnAttacked(CharacterFacade source)
    {
        _baseSM.Trigger("Attacked");
        Context.TargetTransform = source.transform;
    }
    bool WantToFlee()
    {
        return (_health.CurrentValue / _health.BaseValue * 100) < _healthPercentToFlee;
    }
    void Look()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, _lookDist, _targetLayer);
        Context.CharsInSight.Clear();
        foreach (var item in hits)
        {
            CharacterFacade character = item.transform.GetComponent<CharacterFacade>();
            if (Context.CharsInSight.Contains(character) == false && character != _character && character != null)
            {
                Context.CharsInSight.Add(character);
            }

        }

    }
    float _lastRotationTargetChange;
    [SerializeField] float _randomRotationInterval = 1;
    void UpdateRandomRotationTargetOnFluttering()
    {
        float minRotationX = -30f;
        float maxRotationX = 30f;

        float minRotationY = -30f;
        float maxRotationY = 30f;
        if (Time.time - _lastRotationTargetChange > _randomRotationInterval)
        {
            // Генерируем случайные углы в пределах настроенных значений
            float randomXRotation = Random.Range(minRotationX, maxRotationX);
            float randomYRotation = Random.Range(minRotationY, maxRotationY);

            Vector3 randomDirection = Quaternion.Euler(randomXRotation, randomYRotation, 0) * Vector3.forward;

            Context.TargetPos = transform.position + randomDirection * 5;
            _lastRotationTargetChange = Time.time;
        }
    }
    void UpdateTarget()
    {
        (TargetType, Transform, float) selectedObject = (TargetType.noTarget, null, float.MinValue);




        ////добыча
        //(CharacterFacade, float) selectedCharacter = default;
        //foreach (var item in _charsInSight)
        //{
        //    float tempV = EnemyValue(item.transform);
        //    if (tempV > selectedCharacter.Item2)
        //    {
        //        selectedCharacter = (item, tempV);
        //    }
        //}
        //if (selectedCharacter.Item2 > selectedObject.Item3)
        //{
        //    selectedObject = (TargetType.enemy, selectedCharacter.Item1.transform, selectedCharacter.Item2);
        //}

        ////угроза
        //(CharacterFacade, float) selectedFleeCharacter = default;
        //foreach (var item in _charsInSight)
        //{
        //    float tempV = FleeValue(item);
        //    if (tempV > selectedFleeCharacter.Item2)
        //    {
        //        selectedFleeCharacter = (item, tempV);
        //    }
        //}
        //if (selectedFleeCharacter.Item2 > selectedObject.Item3)
        //{
        //    selectedObject = (TargetType.danger, selectedFleeCharacter.Item1.transform, selectedFleeCharacter.Item2);
        //}

        ////оазис
        //(Transform, float) selectedFoodSource = default;
        //foreach (var item in _foodSourceInSight)
        //{
        //    float tempV = FoodSourceValue(item.transform);
        //    if (tempV > selectedFoodSource.Item2)
        //    {
        //        selectedFoodSource = (item.transform, tempV);
        //    }
        //}
        //if (selectedFoodSource.Item2 > selectedObject.Item3)
        //{
        //    selectedObject = (TargetType.foodSource, selectedFoodSource.Item1, selectedFoodSource.Item2);
        //}

        selectedObject = (TargetType.enemy, PlayerManager.Instance.Player.transform, 100);

        //случайная точка в мире
        if (selectedObject.Item3 < 50)
        {
            selectedObject = (TargetType.worldPos, null, 50);
            Context.TargetPos = GetRandomWorldPoint();
        }

        Context.TargetType = selectedObject.Item1;
        Context.TargetTransform = selectedObject.Item2;
    }
    float EnemyValue(Transform t) => 100 - Vector3.Distance(transform.position, t.position);
    float FleeValue(CharacterFacade enemy) => (_character.health.CurrentHealth / _character.health.MaxHealth * 100 + 1);

    Vector3 GetRandomWorldPoint()
    {
        Vector3 randomInsideSphere = Random.insideUnitSphere * _roamRadius;
        RaycastHit hit;
        if (Physics.Linecast(transform.position, randomInsideSphere, out hit, _groundLayer))
        {
            return hit.point;
        }
        else
        {
            return randomInsideSphere;
        }
    }
}
public enum StateT
{
    idle,
    moveToPos,
    flee,
    think,
    fight,
    chase,
    fightThink,
    strafing,
    fightRunAway,
    stand
}
public class AggroEvent
{
    public CharacterFacade character;
    public Transform target;
}