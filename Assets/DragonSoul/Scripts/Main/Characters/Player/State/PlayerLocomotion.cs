using UnityEngine;

namespace Player
{
    public class PlayerLocomotion : IState<PlayerStateID>
    {
        public PlayerStateID StateID => PlayerStateID.Locomotion;
        readonly PlayerCore core;
        readonly DashCooldownManager dashCooldownManager;
        InputReciver Input => InputReciver.Instance;
        Vector3 moveDirection;
        const float TransitionDuration = 0.2f;
        const float RotationSpeed = 10f;

        public PlayerLocomotion(PlayerCore core, DashCooldownManager dashCooldownManager)
        {
            this.core = core;
            this.dashCooldownManager = dashCooldownManager;
        }

        public void Enter()
        {
            core.Animator.applyRootMotion = false;
            core.Animator.CrossFade("Locomotion", TransitionDuration);
        }

        public void Update()
        {
            // ガードステートに遷移
            if (Input.Guard)
            {
                core.StateMachine.ChangeState(PlayerStateID.Guard);
            }

            // ダッシュステートに遷移
            if (Input.Dash && dashCooldownManager.CanDash)
            {
                core.StateMachine.ChangeState(PlayerStateID.Dash);
            }

            if (Input.AttackNormal)
            {
                core.StateMachine.ChangeState(PlayerStateID.AttackNormal1);
            }

            // アニメーションの更新
            core.Animator.SetFloat("Speed", Input.Move.magnitude * core.MoveSpeed, 0.2f, Time.fixedDeltaTime);
        }

        public void FixedUpdate()
        {
            // 移動方向を更新
            Quaternion cameraRotation = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);
            moveDirection = cameraRotation * new Vector3(Input.Move.x, 0, Input.Move.y);

            // 移動方向に回転
            if (moveDirection.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
                core.transform.rotation = Quaternion.Slerp(core.transform.rotation, targetRotation, RotationSpeed * Time.fixedDeltaTime);
            }
    
            core.Rb.velocity = core.MoveSpeed * moveDirection;
        }

        public void Exit()
        {
            core.Rb.velocity = Vector3.zero;
        }
    }
}

