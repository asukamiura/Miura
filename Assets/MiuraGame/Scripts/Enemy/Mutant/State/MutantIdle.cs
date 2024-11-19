using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutantIdle : IState<MutantStateID>
{
    public MutantStateID StateID => MutantStateID.Idle;
    private StateMachine<MutantStateID> stateMachine;
    private MutantCore MutantCore;
    private Animator anim => MutantCore.animator;
    private float timer = 0f;

    public MutantIdle(MutantCore mutantCore, StateMachine<MutantStateID> stateMachine)
    {
        this.stateMachine = stateMachine;
        this.MutantCore = mutantCore;
    }

    public void Enter()
    {
        anim.applyRootMotion = false;
        anim.CrossFade("Idle", 0.1f, 0, 0);
    }

    public void Execute()
    {
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        // アニメーションが終わったらIdleStateに遷移
        //if (stateInfo.normalizedTime >= 1)
        //{
        //}
        timer += Time.deltaTime;
        if (timer > 5f)
        {
            stateMachine.ChangeState(MutantStateID.Attack1);
        }
        
    }

    public void Exit()
    {
        timer = 0f;
    }

}
