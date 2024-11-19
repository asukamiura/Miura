using UnityEngine;

public class PlayerParry : IState<PlayerStateID>
{
    private StateMachine<PlayerStateID> stateMachine;
    public PlayerStateID StateID => PlayerStateID.Parry;
    InputReciver input => InputReciver.Instance;
    private PlayerCore core;
    private Animator anim => core.animator;
    private Rigidbody rb => core.rb;
    private Transform transform => core.transform;
    private bool successParry => core.successParry;

    public PlayerParry(PlayerCore core, StateMachine<PlayerStateID> stateMachine)
    {
        this.stateMachine = stateMachine;
        this.core = core;
    }

    public void Enter()
    {
        //anim.applyRootMotion = true;
        anim.CrossFade("Parry",0,0,0);
    }

    public void Execute()
    {
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.normalizedTime >= 1f)
        {
            stateMachine.ChangeState(PlayerStateID.Idle);
        }
        if (successParry)
        {
            stateMachine.ChangeState(PlayerStateID.ParrySuccess);
        }
    }

    public void Exit()
    {
        core.successParry = false;
    }
}
