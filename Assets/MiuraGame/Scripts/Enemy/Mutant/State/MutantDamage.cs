using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutantDamage : IState<MutantStateID>
{
    public MutantStateID StateID => MutantStateID.Damage;
    private StateMachine<MutantStateID> stateMachine;
    private MutantCore core;

    public MutantDamage(MutantCore core)
    {
        this.core = core;
    }

    public void Enter()
    {
        core.animator.applyRootMotion = true;
        core.animator.CrossFade("Damage", 0.1f, 0, 0);
    }

    public void Execute()
    {
        AnimatorStateInfo stateInfo = core.animator.GetCurrentAnimatorStateInfo(0);
        // アニメーションが終わったらIdleStateに遷移
        if (stateInfo.IsName("Attack1"))
        {
            if (stateInfo.normalizedTime >= 1)
            {
                stateMachine.ChangeState(MutantStateID.Idle);
            }
        }
    }

    public void Exit()
    {

    }

}
