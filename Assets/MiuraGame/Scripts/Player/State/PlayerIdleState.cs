using UnityEngine;

public class PlayerIdleState : IState
{
    private PlayerStateMachine stateMachine;
    public PlayerStateID StateID => PlayerStateID.Idle;
    InputReciver input => InputReciver.Instance;
    private PlayerCore core;
    private Animator anim => core.animator;

    public PlayerIdleState(PlayerCore core, PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        this.core = core;  
    }

    public void Enter()
    {
        Debug.Log("Enter Idle");
        anim.applyRootMotion = false;
    }

    public void Execute()
    {
        
        if (input.Move != Vector2.zero)
        {
            stateMachine.ChangeState(PlayerStateID.Move);
        }

        if (input.Dodge)
        {
            stateMachine.ChangeState(PlayerStateID.Dodge);
        }

        if (input.Parry)
        {
            stateMachine.ChangeState(PlayerStateID.Parry);
        }

        if (input.AttackNormal)
        {
            stateMachine.ChangeState(PlayerStateID.AttackNormal1);
        }
    }

    public void Exit() 
    {

    }
}
