using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutantMove : IState<MutantStateID>
{
    public MutantStateID StateID => MutantStateID.Move;
    private MutantCore core;
    private float timer = 0f;
    private float moveSpeed = 1;
    private float rotationSpeed = 3f;
    private float attackDistance = 3;
    private float attackAngle = 10;

    public MutantMove(MutantCore core)
    {
        this.core = core;
    }

    public void Enter()
    {
        core.animator.applyRootMotion = false;
        core.animator.CrossFade("Move", 0.1f, 0, 0);

    }

    public void Execute()
    {
        Vector3 direction = core.player.transform.position - core.transform.position;
        Quaternion lookAtRotation = Quaternion.LookRotation(direction, Vector3.up);
        core.transform.rotation = Quaternion.Slerp(core.transform.rotation, lookAtRotation, rotationSpeed * Time.deltaTime);  

        core.transform.position = Vector3.MoveTowards(core.transform.position, core.player.transform.position, moveSpeed * Time.deltaTime);

        var angleToPlayer = Vector3.Angle(core.transform.forward, direction);
        float distance = Vector3.Distance(core.transform.position, core.player.transform.position);

        if (distance <= attackDistance && angleToPlayer <= attackAngle)
        {
           int num = Random.Range(1, 3);
            Debug.Log(num);
            if (num == 1)
            {
                core.stateMachine.ChangeState(MutantStateID.Attack1);
            }
            else if (num == 2)
            {
                core.stateMachine.ChangeState(MutantStateID.Attack2);
                num = 0;
            }
        }
    }

    public void Exit()
    {
        timer = 0f;
    }

}
