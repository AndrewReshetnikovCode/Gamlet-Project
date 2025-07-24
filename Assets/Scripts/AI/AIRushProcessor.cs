using UnityEngine;
using UnityHFSM;

public class AIRushProcessor : IFightProcessor
{
    CharacterFacade _t;
    CharacterFacade _c;
    StateMachine<StateT> _sm;
    AIFightSettings _s;
    public AIRushProcessor(AIFightSettings settings)
    {
        _s = settings;

        _sm = new StateMachine<StateT>();

        _sm.AddState(StateT.chase);
        _sm.AddState(StateT.fightMelee);

        _sm.AddTransition(new Transition<StateT>(StateT.chase, StateT.fightMelee, (s) => Vector3.Distance(_t.RootTransform.position, _c.RootTransform.position) < _s.meleeAttackDistance));
    }
    public void AddEnemy(CharacterFacade c)
    {
        _t = c;
    }

    public void AddFriend(CharacterFacade c)
    {

    }

    public void Dispose()
    {
        _t = null;
    }

    public void ProcessFight()
    {
        _sm.OnLogic();
    }
}
