using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAttack2 : IState<SkeletonStateID>
{
    public SkeletonStateID StateID => SkeletonStateID.Attack1;
    private StateMachine<SkeletonStateID> stateMachine;
    private SkeletonCore skeletonCore;

    public SkeletonAttack2(SkeletonCore skeletonCore,StateMachine<SkeletonStateID> stateMachine)
    {
        this.stateMachine = stateMachine;
        this.skeletonCore = skeletonCore;   
    }

    public void Enter()
    {

    }

    public void Execute()
    {

    }

    public void Exit()
    {

    }
}
