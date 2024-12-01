using UnityEngine;

namespace Player
{
    public class PlayerAttackNormal1 : IState<PlayerStateID>
    {
        public PlayerStateID StateID => PlayerStateID.AttackNormal1;
        private PlayerCore core;
        private InputReciver Input => InputReciver.Instance;
        private bool isNextAttack = false;

        public PlayerAttackNormal1(PlayerCore core)
        {
            this.core = core;
        }

        public void Enter()
        {
            //anim.applyRootMotion = true;
            core.Animator.CrossFade("AttackNormal1", 0.1f, 0, 0);

            // カメラの角度に沿って移動
            Quaternion cameraRotation = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);
            Vector3 moveDirection = cameraRotation * new Vector3(Input.Move.x, 0, Input.Move.y).normalized;
            core.transform.rotation = Quaternion.LookRotation(moveDirection, Vector3.up);
        }

        public void Update()
        {
            AnimatorStateInfo stateInfo = core.Animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("AttackNormal1"))
            {
                if (Input.AttackNormal)
                {
                    isNextAttack = true;
                }

                if (isNextAttack && stateInfo.normalizedTime >= 0.6f)
                {
                    core.stateMachine.ChangeState(PlayerStateID.AttackNormal2);
                }
                else if (stateInfo.normalizedTime >= 1)
                {
                    core.stateMachine.ChangeState(PlayerStateID.Idle);
                }
            }
        }

        public void FixedUpdate() { }

        public void Exit()
        {
            isNextAttack = false;
        }
    }
}
