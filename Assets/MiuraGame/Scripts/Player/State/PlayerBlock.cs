using UnityEngine;

namespace Player
{
    public class PlayerBlock : IState<PlayerStateID>
    {
        public PlayerStateID StateID => PlayerStateID.Block;
        private InputReciver input => InputReciver.Instance;
        private PlayerCore core;
        private Vector3 moveTargetPos;
        private float targetDistance = 4;
        private float moveSpeed = 10;
        private float decelerationRate = 0.99f;

        private const float DefaultMoveSpeed = 10;

        public PlayerBlock(PlayerCore core)
        {
            this.core = core;
        }

        public void Enter()
        {
            core.JustGaurd.ActionJustGaurd();
            core.isInvincible = true;
            core.Animator.CrossFade("Block", 0);
        }

        public void Update()
        {
            moveSpeed *= decelerationRate;
            AnimatorStateInfo stateInfo = core.Animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.normalizedTime >= 1f)
            {
                core.stateMachine.ChangeState(PlayerStateID.Idle);
            }            
            if (input.AttackNormal)
            {
                core.stateMachine.ChangeState(PlayerStateID.AttackSpecial2);
            }
        }

        public void FixedUpdate()
        {

        }

        public void Exit()
        {
            core.Rb.velocity = Vector3.zero;
            core.isJustGuard = false;
            core.isInvincible = false;
            moveSpeed = DefaultMoveSpeed;
        }
    }
}
