using System.Xml.Schema;
using UnityEngine;

public class PlayerGuard : IState<PlayerStateID>
{
    private StateMachine<PlayerStateID> stateMachine;
    public PlayerStateID StateID => PlayerStateID.Guard;
    InputReciver input => InputReciver.Instance;
    private PlayerCore core;
    private Animator anim => core.animator;
    private Rigidbody rb => core.rb;
    private Transform transform => core.transform;
    private bool successParry => core.successParry;

    public PlayerGuard(PlayerCore core, StateMachine<PlayerStateID> stateMachine)
    {
        this.stateMachine = stateMachine;
        this.core = core;
    }

    public void Enter()
    {
        //anim.applyRootMotion = true;
        anim.CrossFade("Guard", 0, 0, 0);
    }

    public void Execute()
    {
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        float currentFrame = stateInfo.normalizedTime;
        if (stateInfo.normalizedTime >= 1f)
        {
            stateMachine.ChangeState(PlayerStateID.Idle);
        }
        if (successParry)
        {
            stateMachine.ChangeState(PlayerStateID.Block);
            if (currentFrame > 0 && currentFrame < 0.2f)
            {
                core.TimingUIShow("Slow");
            }
            else if (currentFrame >= 0.2f && currentFrame < 0.8f)
            {
                core.TimingUIShow("Just");
            }
            else if (currentFrame >= 0.8f && currentFrame < 1)
            {
                core.TimingUIShow("Fast");
            }
        }
    }

    public void Exit()
    {
        core.successParry = false;
    }
}
