using UnityEngine;

namespace Player
{
    public class PlayerLocomotion : IState<PlayerStateID>
    {
        public PlayerStateID StateID => PlayerStateID.Locomotion;
        PlayerCore core;
        InputReciver Input => InputReciver.Instance;
        Vector3 moveDirection;

        public PlayerLocomotion(PlayerCore core)
        {
            this.core = core;
        }

        public void Enter()
        {
            core.Animator.applyRootMotion = false;
            core.Animator.CrossFade("Locomotion", 0.2f, 0, 0);
        }

        public void Update()
        {
            // ガードステートに遷移
            if (Input.Guard)
            {
                core.stateMachine.ChangeState(PlayerStateID.Guard);
            }

            // ダッシュステートに遷移
            if (Input.Dash)
            {
                core.stateMachine.ChangeState(PlayerStateID.Dash);
            }

            if (Input.AttackNormal)
            {
                core.stateMachine.ChangeState(PlayerStateID.AttackNormal1);
            }

            // 移動方向を更新
            Quaternion cameraRotation = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);
            moveDirection = cameraRotation * new Vector3(Input.Move.x, 0, Input.Move.y).normalized;

            core.Animator.SetFloat("Speed", Mathf.Clamp(core.Rb.velocity.magnitude, 0, 5), 0.1f, Time.deltaTime);
        }

        public void FixedUpdate()
        {
            if (moveDirection.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
                core.transform.rotation = Quaternion.Slerp(core.transform.rotation, targetRotation, 10 * Time.fixedDeltaTime);
            }

            Vector3 velocity = moveDirection * core.MoveSpeed;
            velocity.y = core.Rb.velocity.y;
            core.Rb.velocity = velocity;
        }

        public void Exit()
        {
            core.Rb.velocity = Vector3.zero;
            //core.Animator.applyRootMotion = true;
        }
    }
}

