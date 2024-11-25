using UnityEngine;

public class PlayerMove : IState<PlayerStateID>
{
    public PlayerStateID StateID => PlayerStateID.Move;
    private PlayerCore core;
    InputReciver input => InputReciver.Instance;
    Quaternion targetRotation;

    public PlayerMove(PlayerCore core)
    {
        this.core = core;
    }

    void Awake()
    {
        targetRotation = core.transform.rotation;
    }

    public void Enter()
    {
        core.animator.applyRootMotion = false;
        core.animator.CrossFade("Locomotion", 0.2f, 0, 0);
    }

    public void Execute()
    {
        // ƒJƒƒ‰‚ÌŠp“x‚É‰ˆ‚Á‚ÄˆÚ“®
        Quaternion cameraRotation = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);
        Vector3 moveDirection = cameraRotation * new Vector3(input.Move.x, 0, input.Move.y).normalized;

        if (moveDirection.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            core.transform.rotation = Quaternion.Slerp(core.transform.rotation, targetRotation, 10 * Time.deltaTime);
        }

        core.rb.velocity = moveDirection * core.moveSpeed;

        if (input.Move == Vector2.zero)
        {
            core.stateMachine.ChangeState(PlayerStateID.Idle);
        }

        if (input.Dodge)
        {
            core.stateMachine.ChangeState(PlayerStateID.Dodge);
        }

        if (input.AttackCharge)
        {
            core.justPointManager.UseJustPoints(1);
            core.stateMachine.ChangeState(PlayerStateID.AttackCharge);
            input.countTime = 0;
        }
        else if (input.AttackNormal)
        {
            core.stateMachine.ChangeState(PlayerStateID.AttackNormal1);
            input.countTime = 0;
        }
    }

    public void Exit()
    {
        core.rb.velocity = Vector3.zero;
    }
}
