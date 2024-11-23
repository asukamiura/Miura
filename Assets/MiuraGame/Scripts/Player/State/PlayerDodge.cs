using UnityEngine;

public class PlayerDodge : IState<PlayerStateID>
{
    public PlayerStateID StateID => PlayerStateID.Dodge;
    private InputReciver input => InputReciver.Instance;
    private PlayerCore core;
    private bool isNextAttack = false;

    public PlayerDodge(PlayerCore core)
    {
        this.core = core;
    }

    public void Enter()
    {
        core.judgeDodgeCollider.enabled = true;
        core.animator.applyRootMotion = true;
        // 移動の入力がなかった場合、バックステップ
        if(input.Move == Vector2.zero)
        {
            core.animator.CrossFade("BackStep",0,0,0);
        }
        else
        {
            core.animator.CrossFade("Dodge",0,0,0);
        }
    }

    public void Execute()
    {
        AnimatorStateInfo stateInfo = core.animator.GetCurrentAnimatorStateInfo(0);
        float currentTime = stateInfo.normalizedTime;

        if (input.AttackNormal && core.isJustDodge)
        {
            isNextAttack = true;
        }

        if (isNextAttack && stateInfo.normalizedTime >= 0.7f)
        {
            core.stateMachine.ChangeState(PlayerStateID.AttackSpecial1);
        }
        else if (stateInfo.normalizedTime >= 0.8f)
        {
            core.stateMachine.ChangeState(PlayerStateID.Idle);
        }

    }

    public void Exit()
    {
        isNextAttack = false;
    }
}
