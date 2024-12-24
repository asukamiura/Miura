using UnityEngine;

namespace Player
{
    public class PlayerAttackSpecial2 : IState<PlayerStateID>
    {
        public PlayerStateID StateID => PlayerStateID.AttackSpecial2;
        private PlayerCore core;
        private InputReciver Input => InputReciver.Instance;

        public PlayerAttackSpecial2(PlayerCore core)
        {
            this.core = core;
        }

        public void Enter()
        {
            core.isInvincible = true;
            core.attackCorrectionManager.CorrectionAttack();
            core.Animator.applyRootMotion = true;
            // アニメーションの遷移
            core.Animator.CrossFade("AttackSpecial2", 0.1f, 0, 0.1f);
        }

        public void Update()
        {
            AnimatorStateInfo stateInfo = core.Animator.GetCurrentAnimatorStateInfo(0);
            // アニメーションが終わったらIdleStateに遷移
            if (stateInfo.normalizedTime >= 0.7)
            {
                core.stateMachine.ChangeState(PlayerStateID.Idle);
            }
        }

        public void FixedUpdate() { }

        public void Exit()
        {
            core.isInvincible = false;
            core.AttackEnd();
        }
    }
}
