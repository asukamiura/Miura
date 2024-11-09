using System.Collections;
using System.Collections.Generic;
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
        anim.SetTrigger("Attack");
        Debug.Log(3);
    }

    public void Execute()
    {
      
    }

    public void Exit()
    {
        //anim.ResetTrigger("AttackNormal");
    }
}
