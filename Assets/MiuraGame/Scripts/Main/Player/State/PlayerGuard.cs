using UnityEngine;

namespace Player
{
    public class PlayerGuard : IState<PlayerStateID>
    {
        public PlayerStateID StateID => PlayerStateID.Guard;
        InputReciver input => InputReciver.Instance;
        PlayerCore core;

        private const int GetJustPoints = 1;    // ジャスト回避成功時に得るジャストポイント量
        private const float LateThreshold = 0.2f;   // 遅すぎる判定のしきい値
        private const float JustStartThreshold = 0.2f;  // ジャスト判定の開始時間
        private const float JustEndThreshold = 0.8f;    // ジャスト判定の終了時間
        private const float FastThreshold = 0.8f;   // 速すぎる判定のしきい値
        private const float AnimationEndThreshold = 1f; // アニメーション終了のしきい値

        public PlayerGuard(PlayerCore core)
        {
            this.core = core;
        }

        public void Enter()
        {
            // anim.applyRootMotion = true;
            core.attackAssist.CorrectionAttack();
            core.Animator.CrossFade("Guard", 0, 0, 0);
        }

        public void Update()
        {
            AnimatorStateInfo stateInfo = core.Animator.GetCurrentAnimatorStateInfo(0);
            float normalizedTime = stateInfo.normalizedTime;

            if (normalizedTime >= AnimationEndThreshold)
            {
                core.stateMachine.ChangeState(PlayerStateID.Idle);
            }

            if (core.isJustGuard)
            {
                if (normalizedTime > 0 && normalizedTime < LateThreshold)
                {
                    core.TimingUIShow("Late");
                }
                else if (normalizedTime >= JustStartThreshold && normalizedTime < JustEndThreshold)
                {
                    core.justGuardCount++;
                    core.TimingUIShow("Just");
                    core.justPointManager.AddJustPoints(GetJustPoints);
                    Debug.Log("Just!!!");
                }
                else if (normalizedTime >= FastThreshold && normalizedTime < AnimationEndThreshold)
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
