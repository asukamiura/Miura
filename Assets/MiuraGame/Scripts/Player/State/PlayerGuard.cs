using UnityEngine;

namespace Player
{
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
            core.Animator.CrossFade("Guard", 0, 0, 0);
        }

        public void Update()
        {
            AnimatorStateInfo stateInfo = core.Animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.normalizedTime >= 1f)
            {
                core.stateMachine.ChangeState(PlayerStateID.Idle);
            }
            if (core.isJustGuard)
            {
                if (stateInfo.normalizedTime > 0 && stateInfo.normalizedTime < 0.2f)
                {
                    core.TimingUIShow("Late");
                }
                else if (stateInfo.normalizedTime >= 0.2f && stateInfo.normalizedTime < 0.8f)
                {
                    core.justGaurdCount++;
                    core.TimingUIShow("Just");
                    core.justPointManager.AddJustPoints(getJustPoints);
                }
                else if (stateInfo.normalizedTime >= 0.8f && stateInfo.normalizedTime < 1)
                {
                    core.TimingUIShow("Fast");
                }
                core.stateMachine.ChangeState(PlayerStateID.Block);
            }
        }

        public void FixedUpdate() { }

        public void Exit()
        {
            core.isJustGuard = false;
        }
    }
}
