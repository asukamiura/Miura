using UnityEngine;

namespace Player
{
    public class PlayerLocomotion : IState<PlayerStateID>
    {
        public PlayerStateID StateID => PlayerStateID.Locomotion;
        PlayerCore core;
        InputReciver Input => InputReciver.Instance;

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

            core.Animator.SetFloat("Speed", Mathf.Clamp(core.Rb.velocity.magnitude, 0, 5), 0.1f, Time.deltaTime);
        }

        public void FixedUpdate()
        {
            // カメラの角度に沿って移動
            Quaternion cameraRotation = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);
            Vector3 moveDirection = cameraRotation * new Vector3(Input.Move.x, 0, Input.Move.y).normalized;

            if (moveDirection.magnitude > 0.1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
                core.transform.rotation = Quaternion.Slerp(core.transform.rotation, targetRotation, 10 * Time.deltaTime);
            }

            core.Rb.velocity = moveDirection * core.MoveSpeed;
        }

        public void Exit()
        {
            //core.Rb.velocity = Vector3.zero;
            //core.Animator.applyRootMotion = true;
        }
    }
}

