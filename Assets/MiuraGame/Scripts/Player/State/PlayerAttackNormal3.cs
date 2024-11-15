using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerAttackNormal3 : IState<PlayerStateID>
{
    public PlayerStateID StateID => PlayerStateID.AttackNormal3;
    private StateMachine<PlayerStateID> stateMachine;
    private PlayerCore core;
    InputReciver input => InputReciver.Instance;
    private Animator anim => core.animator;

    public PlayerAttackNormal3(PlayerCore core, StateMachine<PlayerStateID> stateMachine)
    {
        this.stateMachine = stateMachine;
        this.core = core;
    }

    public void Enter()
    {
        // アニメーションの遷移
        anim.CrossFade("AttackNormal3", 0.1f, 0, 0.1f);
    }

    public void Execute()
    {
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        // アニメーションが終わったらIdleStateに遷移
        if (stateInfo.normalizedTime >= 1)
        {
            stateMachine.ChangeState(PlayerStateID.Idle);
        }
    }

    public void Exit()
    {
        
    }
}
