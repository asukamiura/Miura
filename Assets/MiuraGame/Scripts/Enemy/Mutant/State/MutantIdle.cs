using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutantIdle : IState<MutantStateID>
{
    public MutantStateID StateID => MutantStateID.Idle;
    private MutantCore core;
    private float timer = 0f;

    public MutantIdle(MutantCore core)
    {
        this.core = core;
    }

    public void Enter()
    {
        core.animator.applyRootMotion = false;
        core.animator.CrossFade("Idle", 0.1f, 0, 0);
    }

    public void Execute()
    {
        AnimatorStateInfo stateInfo = core.animator.GetCurrentAnimatorStateInfo(0);
        // アニメーションが終わったらIdleStateに遷移
        //if (stateInfo.normalizedTime >= 1)
        //{
        //}
        timer += Time.deltaTime;
        if (timer > 3f)
        {
            core.stateMachine.ChangeState(MutantStateID.Attack1);
        }
        
    }

    public void Exit()
    {
        timer = 0f;
    }

}
