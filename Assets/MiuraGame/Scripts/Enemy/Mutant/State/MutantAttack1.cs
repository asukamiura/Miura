using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutantAttack1 : IState<MutantStateID>
{
    public MutantStateID StateID => MutantStateID.Attack1;
    private StateMachine<MutantStateID> stateMachine;
    private MutantCore MutantCore;
    private Animator anim => MutantCore.animator;

    public MutantAttack1(MutantCore mutantCore, StateMachine<MutantStateID> stateMachine)
    {
        this.stateMachine = stateMachine;
        this.MutantCore = mutantCore;
    }

    public void Enter()
    {
        anim.applyRootMotion = true;
        anim.CrossFade("Attack1", 0.1f, 0, 0);
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
