using UnityEngine;

namespace Player
{
    public class PlayerDash : IState<PlayerStateID>
    {
        public PlayerStateID StateID => PlayerStateID.Dash;
        readonly PlayerCore core;
        readonly AnimationController animationController;
        readonly DashCooldownManager dashCooldownManager;
        InputReciver Input => InputReciver.Instance;
        string animationStateName;
        Vector3 targetDirection;

        const float DashSpeed = 10f;      // ダッシュ速度
        const string DashFrontAnimationStateName = "DashFront";
        const string DashBackAnimationStateName = "DashBack";
        const float TransitionThreshold = 0.3f;  // 遷移を開始するアニメーションの進捗率

        public PlayerDash(PlayerCore core, AnimationController animationController, DashCooldownManager dashCooldownManager)
        {
            this.core = core;
            this.animationController = animationController;
            this.dashCooldownManager = dashCooldownManager;
        }

        public void Enter()
        {
            core.CreateDodgeCollider();
            dashCooldownManager.StartCooldown();

            if (Input.Move == Vector2.zero)
            {
                animationController.PlayAniamtion(DashBackAnimationStateName);
                animationStateName = DashBackAnimationStateName;

                targetDirection = -core.transform.forward;
            }
            else
            {
                // 移動方向を更新
                Quaternion cameraRotation = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);
                targetDirection = cameraRotation * new Vector3(Input.Move.x, 0, Input.Move.y).normalized;
                core.transform.rotation = Quaternion.LookRotation(targetDirection, Vector3.up);

                animationController.PlayAniamtion(DashFrontAnimationStateName);
                animationStateName = DashFrontAnimationStateName;
            }
        }

        public void Update()
        {
            if (animationController.IsTimeElapsed(animationStateName, TransitionThreshold))
            {
                core.StateMachine.ChangeState(PlayerStateID.Locomotion);
            }         
        }

        public void FixedUpdate()
        {         
            // 減速カーブ
            float t = Mathf.Clamp01(animationController.StateInfo.normalizedTime);
            float speedFactor = 1f - t * t;
            core.Rb.velocity = DashSpeed * speedFactor * targetDirection;
        }

        public void Exit()
        {
            core.DestroyDodgeCollider();
        }
    }
}
