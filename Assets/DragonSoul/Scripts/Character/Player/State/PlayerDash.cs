using System;
using UnityEngine;

namespace Player
{
    public class PlayerDash : IState<PlayerStateID>
    {
        public PlayerStateID StateID => PlayerStateID.Dash;
        InputReciver Input => InputReciver.Instance;
        PlayerCore core;
        string animationName;

        const float DashSpeed = 10f;        // ダッシュ速度
        const float DashDeceleration = 0.95f; // ダッシュ減速率
        const float AnimationEndThreshold = 0.5f; // アニメーション終了のしきい値

        public PlayerDash(PlayerCore core)
        {
            this.core = core;
        }

        public void Enter()
        {
            if (Input.Move == Vector2.zero)
            {
                core.Animator.CrossFade("DashBack", 0.1f);
                animationName = "DashBack";
                core.Rb.velocity = -core.transform.forward * DashSpeed;
            }
            else
            {
                core.Animator.CrossFade("DashFront", 0.1f);
                animationName = "DashFront";
                core.Rb.velocity = core.transform.forward * DashSpeed;
            }
        }

        public void Update()
        {
            if (core.CurrentStateInfo.normalizedTime >= AnimationEndThreshold && core.CurrentStateInfo.IsName(animationName))
            {
                core.stateMachine.ChangeState(PlayerStateID.Locomotion);
            }
        }

        public void FixedUpdate()
        {
            core.Rb.velocity *= DashDeceleration;
        }

        public void Exit()
        {
            core.Rb.velocity = Vector3.zero;
        }
    }
}
