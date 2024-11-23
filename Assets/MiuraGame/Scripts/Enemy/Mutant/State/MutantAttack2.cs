using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutantAttack2 : IState<MutantStateID>
{
    public MutantStateID StateID => MutantStateID.Attack2;
    private StateMachine<MutantStateID> stateMachine;
    private MutantCore core;

    public MutantAttack2(MutantCore core)
    {
        this.core = core;   
    }

    public void Enter()
    {
        core.animator.applyRootMotion = true;
        core.animator.CrossFade("Attack2", 0.1f, 0, 0);
    }

    public void Execute()
    {
        AnimatorStateInfo stateInfo = core.animator.GetCurrentAnimatorStateInfo(0);
        // アニメーションが終わったらIdleStateに遷移
        if (stateInfo.IsName("Attack2"))
        {
            if (stateInfo.normalizedTime >= 1)
            {
                core.stateMachine.ChangeState(MutantStateID.Idle);
            }
        }
    }

    public void Exit()
    {

    }
}
