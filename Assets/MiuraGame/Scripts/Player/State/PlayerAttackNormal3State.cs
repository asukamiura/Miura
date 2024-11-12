using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerAttackNormal3State : IState
{
    public PlayerStateID StateID => PlayerStateID.AttackNormal3;
    private PlayerStateMachine stateMachine;
    private PlayerCore core;
    InputReciver input => InputReciver.Instance;
    private Animator anim => core.animator;

    public PlayerAttackNormal3State(PlayerCore core, PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        this.core = core;
    }

    public void Enter()
    {
        anim.CrossFade("AttackNormal3", 0.1f, 0, 0.1f);
    }

    public void Execute()
    {
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.normalizedTime >= 1)
        {
            stateMachine.ChangeState(PlayerStateID.Idle);
        }
    }

    public void Exit()
    {
        
    }
}
