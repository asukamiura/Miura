using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCore : MonoBehaviour
{
    private PlayerStateMachine stateMachine = new PlayerStateMachine();
    public Rigidbody rb;
    public Animator animator;

    private void Awake()
    {
        stateMachine.RegisterState(new PlayerIdleState(this,stateMachine));
        stateMachine.RegisterState(new PlayerMoveState(this,stateMachine));
        stateMachine.RegisterState(new PlayerDodgeState(this,stateMachine));
        stateMachine.RegisterState(new PlayerParryState(this,stateMachine));
        stateMachine.RegisterState(new PlayerAttackNormal1State(this,stateMachine));
        stateMachine.RegisterState(new PlayerAttackNormal2State(this,stateMachine));
        stateMachine.RegisterState(new PlayerAttackNormal3State(this,stateMachine));

        stateMachine.Initialize(PlayerStateID.Idle);
    }

    private void Start()
    {
        // StateChangeSMBインスタンスにPlayerStateMachineを渡す
        foreach (var behaviour in animator.GetBehaviours<ChangeStateSMB>())
        {
            behaviour.stateMachine = stateMachine;
        }
    }

    private void Update()
    {
        //Debug.Log(stateMachine.currentState);

        stateMachine.Update();
        animator.SetFloat("Speed", rb.velocity.magnitude,0.1f,Time.deltaTime);
    }
}
