using UnityEngine;

namespace Player
{
    public class PlayerDash : IState<PlayerStateID>
    {
        public PlayerStateID StateID => PlayerStateID.Dash;
        InputReciver Input => InputReciver.Instance;
        PlayerCore core;
        float currentTime;

        const float DashTime = 0.3f;    // ダッシュする時間
        const float DashSpeed = 10f;     // ダッシュ速度
        const int GetJustPoints = 1;    // ジャスト回避成功時に得るジャストポイント量

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
                //core.Rb.AddForce(-core.transform.forward * 1000, ForceMode.Impulse);
            }
            else
            {

                core.Animator.CrossFade("DashFront", 0.1f);
                core.Rb.velocity = core.transform.forward * DashSpeed;
                //core.Rb.AddForce(core.transform.forward * 1000, ForceMode.Impulse);
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
                if (currentTime > 0 && currentTime < 0.05f)
                {
                    core.TimingUIShow("Late");
                }
                else if (currentTime >= 0.05f && currentTime < 0.25f)
                {
                    core.justDodgeCount++;
                    core.TimingUIShow("Just");
                    core.justPointManager.AddJustPoints(GetJustPoints);
                    Debug.Log("JUst!!!");
                }
                else if (currentTime >= 0.25f && currentTime < 0.3f)
                {
                    core.TimingUIShow("Fast");
                }
                core.stateMachine.ChangeState(PlayerStateID.Dodge);
            }
        }

        public void FixedUpdate()
        {
            core.Rb.velocity *= 0.95f; 
        }

        public void Exit()
        {
            core.Rb.velocity = Vector3.zero;
            currentTime = 0;
            core.isJustDodge = false;
        } 
    }
}
