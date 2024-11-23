using UnityEngine;

public class PlayerIdle : IState<PlayerStateID>
{
    public PlayerStateID StateID => PlayerStateID.Idle;
    private InputReciver input => InputReciver.Instance;
    private PlayerCore core;

    public PlayerIdle(PlayerCore core)
    {
        this.core = core;
    }

    public void Enter()
    {
        //Debug.Log("Enter Idle");
        core.animator.applyRootMotion = false;
        core.animator.CrossFade("Locomotion", 0.2f, 0, 0);
    }

    public void Execute()
    {

        if (input.Dodge)
        {
            core.stateMachine.ChangeState(PlayerStateID.Dodge);
        }

        if (input.AttackNormal)
        {
            core.stateMachine.ChangeState(PlayerStateID.AttackNormal1);
        }

        if (input.Move != Vector2.zero)
        {
            core.stateMachine.ChangeState(PlayerStateID.Move);
        }
    }

    public void Exit()
    {

    }
}
