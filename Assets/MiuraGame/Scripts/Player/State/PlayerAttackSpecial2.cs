using UnityEngine;

public class PlayerAttackSpecial2 : IState<PlayerStateID>
{
    public PlayerStateID StateID => PlayerStateID.AttackSpecial2;
    private PlayerCore core;
    private InputReciver input => InputReciver.Instance;

    public PlayerAttackSpecial2(PlayerCore core)
    {
        this.core = core;
    }

    public void Enter()
    {
        core.animator.applyRootMotion = true;
        // アニメーションの遷移
        core.animator.CrossFade("AttackSpecial2", 0.1f, 0, 0.1f);
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
