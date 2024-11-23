using UnityEngine;

public class PlayerBlock : IState<PlayerStateID>
{
    public PlayerStateID StateID => PlayerStateID.Block;
    private InputReciver input => InputReciver.Instance;
    private PlayerCore core;

    public PlayerBlock(PlayerCore core)
    {
        this.core = core;
    }

    public void Enter()
    {
        //anim.applyRootMotion = true;
        core.animator.CrossFade("Block", 0, 0, 0);
        core.rb.AddForce(-core.transform.forward * 4, ForceMode.Impulse);
    }

    public void Execute()
    {
        //rb.velocity *= 0.9f;
        AnimatorStateInfo stateInfo = core.animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.normalizedTime >= 1f)
        {
            core.stateMachine.ChangeState(PlayerStateID.Idle);
        }
        if (input.AttackNormal)
        {
            core.stateMachine.ChangeState(PlayerStateID.AttackSpecial2);
        }
    }

    public void Exit()
    {
        core.rb.velocity = Vector3.zero;
        core.isJustGuard = false;
    }
}
