using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackNormal2State : IState
{
    public PlayerStateID StateID => PlayerStateID.AttackNormal2;
    private PlayerStateMachine stateMachine;
    private PlayerCore core;
    InputReciver input => InputReciver.Instance;
    private Animator anim => core.animator;

    public PlayerAttackNormal2State(PlayerCore core, PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        this.core = core;
    }

    public void Enter()
    {
        anim.SetTrigger("Attack");
    }

    public void Execute()
    {
        if (input.AttackNormal)
        {
            stateMachine.ChangeState(PlayerStateID.AttackNormal3);
        }
    }

    public void Exit()
    {

    }
}
