using UnityEngine;

public class PlayerDodgeState : IState
{
    private PlayerStateMachine stateMachine;
    public PlayerStateID StateID => PlayerStateID.Dodge;
    InputReciver input => InputReciver.Instance;
    private PlayerCore core;
    private Animator anim => core.animator;
    private Rigidbody rb => core.rb;

    public PlayerDodgeState(PlayerCore core, PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        this.core = core;
    }

    public void Enter()
    {
        anim.applyRootMotion = true;
        anim.CrossFade("Dodge",0,0,0);
    }

    public void Execute()
    {
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.normalizedTime >= 0.7f)
        {
            stateMachine.ChangeState(PlayerStateID.Idle);
        }
    }

    public void Exit()
    {

    }
}
