using UnityEditor;
using UnityEngine;

public class PlayerDead : IState<PlayerStateID>
{
    private StateMachine<PlayerStateID> stateMachine;
    public PlayerStateID StateID => PlayerStateID.Dead;
    InputReciver input => InputReciver.Instance;
    private PlayerCore core;
    private Animator anim => core.animator;
    private Rigidbody rb => core.rb;

    public PlayerDead(PlayerCore core, StateMachine<PlayerStateID> stateMachine)
    {
        this.stateMachine = stateMachine;
        this.core = core;
    }

    public void Enter()
    {
        core.AttackEnd();
        anim.applyRootMotion = true;
        anim.CrossFade("Death", 0, 0, 0);
    }

    public void Execute()
    {
       
    }

    public void Exit()
    {

    }
}
