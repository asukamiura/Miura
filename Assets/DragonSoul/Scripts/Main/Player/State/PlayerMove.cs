using UnityEngine;

namespace Player
{
    public class PlayerMove : IState<PlayerStateID>
    {
        public PlayerStateID StateID => PlayerStateID.Move;
        PlayerCore core;
        InputReciver Input => InputReciver.Instance;

        public PlayerMove(PlayerCore core)
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
            if (Input.Move == Vector2.zero)
            {
                core.stateMachine.ChangeState(PlayerStateID.Idle);
            }

            if (Input.Dash)
            {
                core.stateMachine.ChangeState(PlayerStateID.Dash);
            }

            if (Input.AttackNormal)
            {
                core.stateMachine.ChangeState(PlayerStateID.AttackNormal1);
            }
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
        }
    }
}

