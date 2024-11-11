using UnityEngine;

public class PlayerDamageState : IState
{
    private PlayerStateMachine stateMachine;
    public PlayerStateID StateID => PlayerStateID.Damage;
    InputReciver input => InputReciver.Instance;
    private PlayerCore core;
    private Animator anim => core.animator;
    private Rigidbody rb => core.rb;

    public PlayerDamageState(PlayerCore core, PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        this.core = core;
    }

    public void Enter()
    {
        anim.applyRootMotion = true;
        anim.SetTrigger("Damage");
        rb.AddForce(new Vector3(0,0,-50), ForceMode.Impulse);
    }

    public void Execute()
    {

    }

    public void Exit()
    {

    }
}
