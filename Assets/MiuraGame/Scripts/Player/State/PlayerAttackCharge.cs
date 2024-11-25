using UnityEngine;

public class PlayerAttackCharge : IState<PlayerStateID>
{
    public PlayerStateID StateID => PlayerStateID.AttackCharge;
    private PlayerCore core;
    private InputReciver input => InputReciver.Instance;

    public PlayerAttackCharge(PlayerCore core)
    {
        this.core = core;
    }

    public void Enter()
    {
        core.animator.applyRootMotion = true;
        // アニメーションの遷移
        core.animator.CrossFade("AttackCharge", 0, 0, 0);
    }

    public void Execute()
    {
        AnimatorStateInfo stateInfo = core.animator.GetCurrentAnimatorStateInfo(0);
        // アニメーションが終わったらIdleStateに遷移
        if (stateInfo.normalizedTime >= 1)
        {
            core.stateMachine.ChangeState(PlayerStateID.Idle);
        }
    }

    public void Exit()
    {

    }
}
