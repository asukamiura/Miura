using UnityEngine;

namespace Player
{
    public class PlayerBlock : IState<PlayerStateID>
    {
        public PlayerStateID StateID => PlayerStateID.Block;
        private InputReciver Input => InputReciver.Instance;
        private PlayerCore core;

        private const float DefaultMoveSpeed = 10;

        public PlayerBlock(PlayerCore core)
        {
            this.core = core;
        }

        public void Enter()
        {
            core.JustGuard.ActionJustGuard();
            core.isInvincible = true;
            core.Animator.CrossFade("Block", 0);
        }

        public void Update()
        {
            AnimatorStateInfo stateInfo = core.Animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.normalizedTime >= 1f)
            {
                core.stateMachine.ChangeState(PlayerStateID.Idle);
            }            
            if (Input.AttackNormal)
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
        }
    }
}
