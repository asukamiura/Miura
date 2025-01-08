using UnityEngine;

namespace Player
{
    public class PlayerAttackSpecial2 : IState<PlayerStateID>
    {
        public PlayerStateID StateID => PlayerStateID.AttackSpecial2;
        private PlayerCore core;
        private InputReciver Input => InputReciver.Instance;
        private bool isNextAttack = false;

        private const float NextStateTransitionTime = 0.645f;

        public PlayerAttackSpecial2(PlayerCore core)
        {
            this.core = core;
        }

        public void Enter()
        {
            core.Animator.applyRootMotion = true;
            core.isInvincible = true;
            core.Animator.applyRootMotion = true;
            // アニメーションの遷移
            core.Animator.CrossFade("AttackSpecial2", 0.1f, 0, 0.1f);
        }

        public void Update()
        {
            AnimatorStateInfo stateInfo = core.Animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("AttackSpecial2"))
            {
                if (stateInfo.normalizedTime >= 0.24 && stateInfo.normalizedTime <= 0.31)
                {
                    core.Animator.speed = 0.3f;
                }
                else
                {
                    core.Animator.speed = 1.2f;
                }

                if (stateInfo.normalizedTime >= NextStateTransitionTime && !isNextAttack)
                {
                    isNextAttack = true;
                    // 次のアニメーションに遷移
                    core.Animator.CrossFade("AttackSpecial2_2", 0.1f, 0, 0.17f);
                }
            }
            else if (stateInfo.IsName("AttackSpecial2_2"))
            {
                if (stateInfo.normalizedTime >= 0.54 && stateInfo.normalizedTime <= 0.6)
                {
                    core.Animator.speed = 0.3f;
                }
                else
                {
                    core.Animator.speed = 1.3f;
                }

                if (stateInfo.normalizedTime >= 1)
                {
                    core.stateMachine.ChangeState(PlayerStateID.Idle);
                }
            }
        }

        public void FixedUpdate() { }

        public void Exit()
        {
            isNextAttack = false;
            core.Animator.speed = 1;
            core.isInvincible = false;
            core.AttackEnd();
        }
    }
}
