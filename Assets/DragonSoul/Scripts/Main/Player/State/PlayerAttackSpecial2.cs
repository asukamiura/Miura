using UnityEngine;

namespace Player
{
    public class PlayerAttackSpecial2 : IState<PlayerStateID>
    {
        public PlayerStateID StateID => PlayerStateID.AttackSpecial2;
        PlayerCore core;
        InputReciver Input => InputReciver.Instance;
        bool isNextAttack = false;

        const float NextStateTransitionTime = 0.645f;

        public PlayerAttackSpecial2(PlayerCore core)
        {
            this.core = core;
        }

        public void Enter()
        {
            core.Animator.applyRootMotion = true;
            core.IsInvincible = true;
            core.Animator.applyRootMotion = true;
            // アニメーションの遷移
            core.Animator.CrossFade("AttackSpecial2_1", 0.1f, 0, 0.1f);

            core.powerManager.SetAttackPower("Special1_1");
        }

        public void Update()
        {
            if (core.CurrentStateInfo.IsName("AttackSpecial2_1"))
            {
                if (core.CurrentStateInfo.normalizedTime >= 0.24 && core.CurrentStateInfo.normalizedTime <= 0.31)
                {
                    core.Animator.speed = 0.3f;
                }
                else
                {
                    core.Animator.speed = 1.5f;
                }

                if (core.CurrentStateInfo.normalizedTime >= NextStateTransitionTime && !isNextAttack)
                {
                    isNextAttack = true;
                    // 次のアニメーションに遷移
                    core.Animator.CrossFade("AttackSpecial2_2", 0.1f, 0, 0.5f);
                }
            }
            else if (core.CurrentStateInfo.IsName("AttackSpecial2_2"))
            {
                if (core.CurrentStateInfo.normalizedTime >= 1)
                {
                    core.Animator.CrossFade("AttackSpecial2_3", 0);
                }
            }
            else if (core.CurrentStateInfo.IsName("AttackSpecial2_3"))
            {
                if (core.CurrentStateInfo.normalizedTime >= 1)
                {
                    core.Animator.CrossFade("AttackSpecial2_4", 0);
                }
            }
            else if (core.CurrentStateInfo.IsName("AttackSpecial2_4"))
            {
                if (core.CurrentStateInfo.normalizedTime >= 0.26 && core.CurrentStateInfo.normalizedTime <= 0.33)
                {
                    core.animationController.ChangeAllAnimationSpeed(0.3f);
                }
                else
                {
                    core.animationController.ChangeAnimationSpeed("Player", 1f);
                    core.animationController.ChangeAnimationSpeed("Enemy", 1f);
                }

                if (core.CurrentStateInfo.normalizedTime >= 1)
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
            core.IsInvincible = false;
        }
    }
}
