using UnityEngine;

public class PlayerParrySuccess : IState<PlayerStateID>
{
    private StateMachine<PlayerStateID> stateMachine;
    public PlayerStateID StateID => PlayerStateID.ParrySuccess;
    InputReciver input => InputReciver.Instance;
    private PlayerCore core;
    private Animator anim => core.animator;
    private Rigidbody rb => core.rb;
    private Transform transform => core.transform;

    public PlayerParrySuccess(PlayerCore core, StateMachine<PlayerStateID> stateMachine)
    {
        this.stateMachine = stateMachine;
        this.core = core;
    }

    public void Enter()
    {
        //anim.applyRootMotion = true;
        anim.CrossFade("Block", 0, 0, 0);
        rb.AddForce(-transform.forward * 4, ForceMode.Impulse);
    }

    public void Execute()
    {
        //rb.velocity *= 0.9f;
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.normalizedTime >= 1f)
        {
            stateMachine.ChangeState(PlayerStateID.Idle);
        }
        if (input.AttackNormal)
        {
            stateMachine.ChangeState(PlayerStateID.AttackSpecial2);
        }
    }

    public void Exit()
    {
        rb.velocity = Vector3.zero;
        core.isBlock = false;
    }
}
