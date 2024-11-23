using UnityEditor;
using UnityEngine;

public class PlayerDead : IState<PlayerStateID>
{
    public PlayerStateID StateID => PlayerStateID.Dead;
    private InputReciver input => InputReciver.Instance;
    private PlayerCore core;

    public PlayerDead(PlayerCore core)
    {
        this.core = core;
    }

    public void Enter()
    {
        core.AttackEnd();
        core.animator.applyRootMotion = true;
        core.animator.CrossFade("Death", 0, 0, 0);
    }

    public void Execute()
    {
       
    }

    public void Exit()
    {

    }
}
