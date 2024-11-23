using UnityEngine;

public class PlayerGuard : IState<PlayerStateID>
{
    public PlayerStateID StateID => PlayerStateID.Guard;
    private InputReciver input => InputReciver.Instance;
    private PlayerCore core;
    private const int getJustPoints = 1;    // ジャスト回避成功時に得るジャストポイント量

    public PlayerGuard(PlayerCore core)
    {
        this.core = core;
    }

    public void Enter()
    {
        //anim.applyRootMotion = true;
        core.animator.CrossFade("Guard", 0, 0, 0);
    }

    public void Execute()
    {
        AnimatorStateInfo stateInfo = core.animator.GetCurrentAnimatorStateInfo(0);
        float currentTime = stateInfo.normalizedTime;
        if (stateInfo.normalizedTime >= 1f)
        {
            core.stateMachine.ChangeState(PlayerStateID.Idle);
        }
        if (core.isJustGuard)
        {
            core.stateMachine.ChangeState(PlayerStateID.Block);
            if (currentTime > 0 && currentTime < 0.2f)
            {
                core.TimingUIShow("Slow");
            }
            else if (currentTime >= 0.2f && currentTime < 0.8f)
            {
                core.TimingUIShow("Just");
                core.justPointManager.AddJustPoints(getJustPoints);
            }
            else if (currentTime >= 0.8f && currentTime < 1)
            {
                core.TimingUIShow("Fast");
            }
        }
    }

    public void Exit()
    {
        core.isJustGuard = false;
    }
}
