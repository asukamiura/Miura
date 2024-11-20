using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackNormal1 : IState<PlayerStateID>
{
    public PlayerStateID StateID => PlayerStateID.AttackNormal1;
    private StateMachine<PlayerStateID> stateMachine;
    private PlayerCore core;
    InputReciver input => InputReciver.Instance;
    private Animator anim => core.animator;
    private bool isNextAttack = false;

    public PlayerAttackNormal1(PlayerCore core, StateMachine<PlayerStateID> stateMachine)
    {
        this.stateMachine = stateMachine;
        this.core = core;
    }

    public void Enter()
    {
        //anim.applyRootMotion = true;
        anim.CrossFade("AttackNormal1",0.1f,0,0);
    }

    public void Execute()
    {
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("AttackNormal1"))
        {
            if (input.AttackNormal)
            {
                isNextAttack = true;
            }

            if (isNextAttack && stateInfo.normalizedTime >= 0.6f)
            {
                stateMachine.ChangeState(PlayerStateID.AttackNormal2);
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
