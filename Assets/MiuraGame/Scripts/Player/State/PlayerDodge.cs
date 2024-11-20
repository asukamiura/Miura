using UnityEngine;

public class PlayerDodge : IState<PlayerStateID>
{
    private StateMachine<PlayerStateID> stateMachine;
    public PlayerStateID StateID => PlayerStateID.Dodge;
    InputReciver input => InputReciver.Instance;
    private PlayerCore core;
    private Animator anim => core.animator;
    private Rigidbody rb => core.rb;

    public PlayerDodge(PlayerCore core, StateMachine<PlayerStateID> stateMachine)
    {
        this.stateMachine = stateMachine;
        this.core = core;
    }

    public void Enter()
    {
        anim.applyRootMotion = true;
        // 移動の入力がなかった場合、バックステップ
        if(input.Move == Vector2.zero)
        {
            anim.CrossFade("BackStep",0,0,0);
        }
        else
        {
            anim.CrossFade("Dodge",0,0,0);
        }
    }

    public void Execute()
    {
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.normalizedTime >= 0.7f)
        {
            stateMachine.ChangeState(PlayerStateID.Idle);
        }
    }

    public void Exit()
    {

    }
}
