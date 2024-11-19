using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MutantCore : MonoBehaviour
{
    public Rigidbody rb;
    public Animator animator;
    public new Collider collider;
    public Collider attackCollider;
    public HealthManager healthManager;
    private StateMachine<MutantStateID> stateMachine;
    public event Action<ParryTiming> OnParryTimingChanged;

    private void Awake()
    {
        stateMachine = new StateMachine<MutantStateID>();
        stateMachine.RegisterState(new MutantIdle(this,stateMachine));
        stateMachine.RegisterState(new MutantAttack1(this,stateMachine));
        stateMachine.RegisterState(new MutantAttack2(this,stateMachine));
    }

    private void Start()
    {
        stateMachine.Initialize(MutantStateID.Idle);
        attackCollider.enabled = false;
        //healthManager = GetComponent<HealthManager>();
    }

    private void Update()
    {
        stateMachine.Update();
    }

    public void AttackStart()
    {
        attackCollider.enabled = true;
    }

    public void AttackEnd()
    {
        attackCollider.enabled = false;
    }

    public void ParryTimgChange(ParryTiming timingNum)
    {
        OnParryTimingChanged?.Invoke(timingNum);
    }
}
