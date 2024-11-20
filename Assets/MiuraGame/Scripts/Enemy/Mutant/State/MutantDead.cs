using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutantDead : IState<MutantStateID>
{
    public MutantStateID StateID => MutantStateID.Dead;
    private StateMachine<MutantStateID> stateMachine;
    private MutantCore MutantCore;
    private Animator anim => MutantCore.animator;

    public MutantDead(MutantCore mutantCore, StateMachine<MutantStateID> stateMachine)
    {
        this.stateMachine = stateMachine;
        this.MutantCore = mutantCore;
    }

    public void Enter()
    {
        anim.applyRootMotion = true;
        anim.CrossFade("Death", 0.1f, 0, 0);
    }

    public void Execute()
    {
        
    }

    public void Exit()
    {

    }
}
