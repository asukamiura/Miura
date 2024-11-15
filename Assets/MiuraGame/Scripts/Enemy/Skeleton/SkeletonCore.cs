using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonCore : EnemyCoreBase
{
    private StateMachine<SkeletonStateID> stateMachine;

    private void Awake()
    {
        stateMachine = new StateMachine<SkeletonStateID>();
        stateMachine.RegisterState(new SkeletonAttack1(this,stateMachine));
        stateMachine.RegisterState(new SkeletonAttack2(this,stateMachine));
    }

}
