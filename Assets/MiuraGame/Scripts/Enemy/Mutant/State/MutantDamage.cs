using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutantDamage : IState<MutantStateID>
{
    public MutantStateID StateID => MutantStateID.Damage;
    private StateMachine<MutantStateID> stateMachine;
    private MutantCore MutantCore;
    private Animator anim => MutantCore.animator;

    public MutantDamage(MutantCore mutantCore, StateMachine<MutantStateID> stateMachine)
    {
        this.stateMachine = stateMachine;
        this.MutantCore = mutantCore;
    }

    public void Enter()
    {
        anim.applyRootMotion = true;
        anim.CrossFade("Damage", 0.1f, 0, 0);
    }

    public void Execute()
    {
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
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
