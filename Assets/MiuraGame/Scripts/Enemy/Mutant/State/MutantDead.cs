using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutantDead : IState<MutantStateID>
{
    public MutantStateID StateID => MutantStateID.Dead;
    private MutantCore core;

    public MutantDead(MutantCore core)
    {
        this.core = core;
    }

    public void Enter()
    {
        core.animator.applyRootMotion = true;
        core.animator.CrossFade("Death", 0.1f, 0, 0);
    }

    public void Execute()
    {
        
    }

    public void Exit()
    {

    }
}
