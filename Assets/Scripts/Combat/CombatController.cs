using UnityEngine;


public class CombatController : MonoBehaviour
{
    public string meleeAttackTrigger;
    public string jumpAttackTrigger;
    public string rangeAttackTrigger;



    Animator _a;
    
    public void StartAttackMelee()
    {
        _a.SetTrigger(meleeAttackTrigger);
    }
    public void StartJumpAttack()
    {
        _a.SetTrigger(jumpAttackTrigger);
    }
    public void StartRangeAttack()
    {
        _a.SetTrigger(rangeAttackTrigger);
    }
    public void OnAttackFrame()
    {

    }
}
