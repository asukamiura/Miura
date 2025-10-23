using UnityEngine;

namespace Player
{
    public class PlayerDash : IState<PlayerStateID>
    {
        public PlayerStateID StateID => PlayerStateID.Dash;
        InputReciver Input => InputReciver.Instance;
        PlayerCore core;
        string animationName;
        Vector3 targetDirection;

        const float DashSpeed = 10f;      // ダッシュ速度
        const float AnimationEndThreshold = 0.5f; // アニメーション終了のしきい値
  

        public PlayerDash(PlayerCore core)
        {
            this.core = core;
        }

        public void Enter()
        {
            core.dashCooldownManager.StartCooldown();

            if (Input.Move == Vector2.zero)
            {
                core.Animator.CrossFade("DashBack", 0);
                animationName = "DashBack";

                targetDirection = -core.transform.forward;
            }
            else
            {
                // 移動方向を更新
                Quaternion cameraRotation = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);
                targetDirection = cameraRotation * new Vector3(Input.Move.x, 0, Input.Move.y).normalized;
                core.transform.rotation = Quaternion.LookRotation(targetDirection, Vector3.up);

                core.Animator.CrossFade("DashFront", 0);
                animationName = "DashFront";
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
            if (targetDirection.sqrMagnitude > 0.01f && animationName != "DashBack")
            {
                Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
                core.transform.rotation = Quaternion.Slerp(core.transform.rotation, targetRotation, 5 * Time.fixedDeltaTime);
            }

            // 減速カーブ
            float t = Mathf.Clamp01(core.CurrentStateInfo.normalizedTime);
            float speedFactor = 1f - t * t;
            core.Rb.velocity = DashSpeed * speedFactor * targetDirection;
        }

        public void Exit()
        {
            core.Rb.velocity = Vector3.zero;
        }
    }
}
