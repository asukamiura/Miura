using UnityEngine;

public class PlayerAttackSpecial1 : IState<PlayerStateID>
{
    public PlayerStateID StateID => PlayerStateID.AttackSpecial1;
    private PlayerCore core;
    private InputReciver input => InputReciver.Instance;

    public PlayerAttackSpecial1(PlayerCore core)
    {
        this.core = core;
    }

    public void Enter()
    {
        core.animator.applyRootMotion = true;
        core.animator.speed = 1;
        // アニメーションの遷移
        core.animator.CrossFade("AttackSpecial1", 0.1f, 0, 0);
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
