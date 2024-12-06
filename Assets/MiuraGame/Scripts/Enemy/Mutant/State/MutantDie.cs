using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutantDie : IState<MutantStateID>
{
    public MutantStateID StateID => MutantStateID.Dead;
    private MutantCore core;

    public MutantDie(MutantCore core)
    {
        this.core = core;
    }

    public void Enter()
    {
        core.animator.applyRootMotion = true;
        core.animator.CrossFade("Death", 0.1f, 0, 0);
    }

    public void Update()
    {
        
    }

    public void FixedUpdate() { }

    public void Exit()
    {

    }
}
