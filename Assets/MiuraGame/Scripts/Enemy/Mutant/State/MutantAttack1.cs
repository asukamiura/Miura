using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutantAttack1 : IState<MutantStateID>
{
    public MutantStateID StateID => MutantStateID.Attack1;
    private StateMachine<MutantStateID> stateMachine;
    private MutantCore core;

    public MutantAttack1(MutantCore core)
    {
        this.core = core;
    }

    public void Enter()
    {
        core.animator.applyRootMotion = true;
        core.animator.CrossFade("Attack1", 0.1f, 0, 0);
    }

    public void Update()
    {
        AnimatorStateInfo stateInfo = core.animator.GetCurrentAnimatorStateInfo(0);
        // アニメーションが終わったらIdleStateに遷移
        if (stateInfo.IsName("Attack1"))
        {
            if (stateInfo.normalizedTime >= 1)
            {
                core.stateMachine.ChangeState(MutantStateID.Idle);
            }
        }
    }

    public void FixedUpdate() { }

    public void Exit()
    {

    }
   
}
