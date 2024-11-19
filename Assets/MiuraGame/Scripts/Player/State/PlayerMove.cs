using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : IState<PlayerStateID>
{
    public PlayerStateID StateID => PlayerStateID.Move;
    private StateMachine<PlayerStateID> stateMachine;
    private PlayerCore core;
    InputReciver input => InputReciver.Instance;
    Quaternion targetRotation;
    private float moveSpeed = 5;
    private Rigidbody rb => core.rb;
    private Transform transform => core.transform;
    private Animator anim => core.animator;

    public PlayerMove (PlayerCore core, StateMachine<PlayerStateID> stateMachine)
    {
        this.stateMachine = stateMachine;
        this.core = core;
    }

    void Awake()
    {
        targetRotation = transform.rotation;
    }

    public void Enter()
    {
        anim.applyRootMotion = false;
        anim.CrossFade("Locomotion", 0.2f, 0, 0);
    }

    public void Execute()
    {
        Move();

        if (input.Move == Vector2.zero)
        {
            stateMachine.ChangeState(PlayerStateID.Idle);
        }

        if (input.Dodge)
        {
            stateMachine.ChangeState(PlayerStateID.Dodge);
        }

        //if (input.Parry)
        //{
        //    stateMachine.ChangeState(PlayerStateID.Parry);
        //}

        if (input.AttackNormal)
        {
            stateMachine.ChangeState(PlayerStateID.AttackNormal1);
        }


    }

    public void Exit()
    {
        rb.velocity = Vector3.zero;
    }

    void Move()
    {
        // ƒJƒƒ‰‚ÌŠp“x‚É‰ˆ‚Á‚ÄˆÚ“®
        Quaternion cameraRotation = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);
        Vector3 moveDirection = cameraRotation * new Vector3(input.Move.x, 0, input.Move.y).normalized;

        if (moveDirection.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10 * Time.deltaTime);
        }

        rb.velocity = moveDirection * moveSpeed;
    }
}
