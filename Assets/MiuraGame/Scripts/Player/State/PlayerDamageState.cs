using UnityEditor;
using UnityEngine;

public class PlayerDamageState : IState
{
    private PlayerStateMachine stateMachine;
    public PlayerStateID StateID => PlayerStateID.Damage;
    InputReciver input => InputReciver.Instance;
    private PlayerCore core;
    private Animator anim => core.animator;
    private Rigidbody rb => core.rb;

    public PlayerDamageState(PlayerCore core, PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        this.core = core;
    }

    public void Enter()
    {
        anim.applyRootMotion = true;
        anim.CrossFade("Damage", 0, 0, 0);
        //rb.AddForce(new Vector3(0,0,-50), ForceMode.Impulse);
    }

    public void Execute()
    {
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Damage"))
        {
            if (stateInfo.normalizedTime >= 1)
            {
                stateMachine.ChangeState(PlayerStateID.Idle);
            }
        }
    }

    public void Exit()
    {

    }
}
