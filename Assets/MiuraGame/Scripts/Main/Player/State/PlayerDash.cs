using UnityEngine;

namespace Player
{
    public class PlayerDash : IState<PlayerStateID>
    {
        public PlayerStateID StateID => PlayerStateID.Dash;
        InputReciver Input => InputReciver.Instance;
        PlayerCore core;
        float currentTime;

        private const float DashTime = 0.3f;        // ダッシュする時間
        private const float DashSpeed = 10f;        // ダッシュ速度
        private const float DashDeceleration = 0.95f; // ダッシュ減速率

        private const int GetJustPoints = 1;        // ジャスト回避成功時に得るジャストポイント量
        private const float LateThreshold = 0.05f;  // 遅すぎる判定のしきい値
        private const float JustStartThreshold = 0.05f; // ジャスト判定の開始時間
        private const float JustEndThreshold = 0.25f;   // ジャスト判定の終了時間
        private const float FastThreshold = 0.25f;  // 速すぎる判定のしきい値
        private const float DashEndThreshold = 0.3f; // ダッシュ終了時間

        public PlayerDash(PlayerCore core)
        {
            this.core = core;
        }

        public void Enter()
        {
            if (Input.Move == Vector2.zero)
            {
                core.Animator.CrossFade("DashBack", 0.1f);
                core.Rb.velocity = -core.transform.forward * DashSpeed;
            }
            else
            {
                core.Animator.CrossFade("DashFront", 0.1f);
                core.Rb.velocity = core.transform.forward * DashSpeed;
            }
        }

        public void Update()
        {
            currentTime += Time.deltaTime;
            if (currentTime >= DashTime)
            {
                core.stateMachine.ChangeState(PlayerStateID.Move);
            }

            if (core.isJustDodge)
            {
                if (currentTime > 0 && currentTime < LateThreshold)
                {
                    core.TimingUIShow("Late");
                }
                else if (currentTime >= JustStartThreshold && currentTime < JustEndThreshold)
                {
                    core.justDodgeCount++;
                    core.TimingUIShow("Just");
                    core.justPointManager.AddJustPoints(GetJustPoints);
                    Debug.Log("Just!!!");
                }
                else if (currentTime >= FastThreshold && currentTime < DashEndThreshold)
                {
                    core.TimingUIShow("Fast");
                }

                core.stateMachine.ChangeState(PlayerStateID.Dodge);
            }
        }

        public void FixedUpdate()
        {
            core.Rb.velocity *= DashDeceleration;
        }

        public void Exit()
        {
            core.Rb.velocity = Vector3.zero;
            currentTime = 0;
            core.isJustDodge = false;
        }
    }
}
