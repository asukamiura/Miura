using System;
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

        public static event Action<string> OnJudgeGuardTiming;

        public PlayerGuard(PlayerCore core)
        {
            this.core = core;
        }

        public void Enter()
        {
            core.attackAssist.CorrectionAttack();
            core.Animator.CrossFade("Guard", 0);

            core.playerEventManager.TriggerGuard();
        }

        public void Update()
        {
            float normalizedTime = core.CurrentStateInfo.normalizedTime;

            if (normalizedTime >= AnimationEndThreshold && core.CurrentStateInfo.IsName("Guard"))
            {
                core.stateMachine.ChangeState(PlayerStateID.Locomotion);
            }

            if (core.IsJustGuard)
            {
                GuardTiming timing = JudgeGuard(normalizedTime); 
                
                if (timing == GuardTiming.Just)
                {
                    core.justPointManager.AddJustPoint(GetJustPoints);
                }

                OnJudgeGuardTiming?.Invoke(timing.ToString());

                core.stateMachine.ChangeState(PlayerStateID.Block);               
            }
        }

        public void FixedUpdate() { }

        public void Exit()
        {
            core.IsJustGuard = false;
        }

        enum GuardTiming{ None, Fast, Just, Late }

        GuardTiming JudgeGuard(float normalizedTime)
        {
            if (normalizedTime >= 0 && normalizedTime < LateThreshold)
            {
                return GuardTiming.Late;                
            }
            else if (normalizedTime >= JustStartThreshold && normalizedTime < JustEndThreshold)
            {
                return GuardTiming.Just;
            }
            else if (normalizedTime >= FastThreshold && normalizedTime < AnimationEndThreshold)
            {
                return GuardTiming.Fast;
            }
            return GuardTiming.None;
        }
    }
}
