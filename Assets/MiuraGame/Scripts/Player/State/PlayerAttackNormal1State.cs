using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackNormal1State : IState
{
    public PlayerStateID StateID => PlayerStateID.AttackNormal1;
    private PlayerStateMachine stateMachine;
    private PlayerCore core;
    InputReciver input => InputReciver.Instance;
    private Animator anim => core.animator;
    //private bool isNextNormalAttack;

    public PlayerAttackNormal1State(PlayerCore core, PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        this.core = core;
    }

    public void Enter()
    {
        anim.applyRootMotion = true;
        anim.SetTrigger("Attack");
    }

    public void Execute()
    {
        if (input.AttackNormal)
        {
            stateMachine.ChangeState(PlayerStateID.AttackNormal2);
            //anim.SetTrigger("Attack");
        }
    }

    public void Exit()
    {
        
    }
}
