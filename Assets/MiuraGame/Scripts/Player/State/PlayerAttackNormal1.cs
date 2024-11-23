using UnityEngine;

public class PlayerAttackNormal1 : IState<PlayerStateID>
{
    public PlayerStateID StateID => PlayerStateID.AttackNormal1;
    private PlayerCore core;
    private InputReciver input => InputReciver.Instance;
    private bool isNextAttack = false;

    public PlayerAttackNormal1(PlayerCore core)
    {
        this.core = core;
    }

    public void Enter()
    {
        //anim.applyRootMotion = true;
        core.animator.CrossFade("AttackNormal1", 0.1f, 0, 0);
    }

    public void Execute()
    {
        AnimatorStateInfo stateInfo = core.animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("AttackNormal1"))
        {
            if (input.AttackNormal)
            {
                isNextAttack = true;
            }

            if (isNextAttack && stateInfo.normalizedTime >= 0.6f)
            {
                core.stateMachine.ChangeState(PlayerStateID.AttackNormal2);
            }
            else if (stateInfo.normalizedTime >= 1)
            {
                core.stateMachine.ChangeState(PlayerStateID.Idle);
            }
        }
    }

    public void Exit()
    {
        isNextAttack = false;
    }
}
