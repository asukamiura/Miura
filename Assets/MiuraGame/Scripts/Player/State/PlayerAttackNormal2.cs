using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackNormal2 : IState<PlayerStateID>
{
    public PlayerStateID StateID => PlayerStateID.AttackNormal2;
    private StateMachine<PlayerStateID> stateMachine;
    private PlayerCore core;
    InputReciver input => InputReciver.Instance;
    private Animator anim => core.animator;
    private bool isNextAttack = false;  // コンボ攻撃を継続フラグ

    public PlayerAttackNormal2(PlayerCore core, StateMachine<PlayerStateID> stateMachine)
    {
        this.stateMachine = stateMachine;
        this.core = core;
    }

    public void Enter()
    {
        // アニメーションの遷移
        anim.CrossFade("AttackNormal2", 0.1f, 0, 0);
    }

    public void Execute()
    {
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("AttackNormal2"))
        {
            if (input.AttackNormal)
            {
                isNextAttack = true;
            }

            if (isNextAttack && stateInfo.normalizedTime >= 0.6f)
            {
                stateMachine.ChangeState(PlayerStateID.AttackNormal3);
            }
            else if (stateInfo.normalizedTime >= 1)
            {
                stateMachine.ChangeState(PlayerStateID.Idle);
            }
        }
    }

    public void Exit()
    {
        isNextAttack = false;
    }
}
