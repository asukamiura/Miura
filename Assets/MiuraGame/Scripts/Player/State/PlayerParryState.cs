using UnityEngine;

public class PlayerParryState : IState
{
    private PlayerStateMachine stateMachine;
    public PlayerStateID StateID => PlayerStateID.Parry;
    InputReciver input => InputReciver.Instance;
    private PlayerCore core;
    private Animator anim => core.animator;
    private Rigidbody rb => core.rb;

    public PlayerParryState(PlayerCore core, PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        this.core = core;
    }

    public void Enter()
    {
        anim.applyRootMotion = true;
        anim.SetTrigger("Parry");
    }

    public void Execute()
    {

    }

    public void Exit()
    {

    }
}
