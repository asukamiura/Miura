using System;
using UnityEngine;

namespace Player
{
    public class PlayerAttackSpecial1_2 : IState<PlayerStateID>
    {
        public PlayerStateID StateID => PlayerStateID.AttackSpecial1_2;
        PlayerCore core;
        InputReciver input => InputReciver.Instance;

        public PlayerAttackSpecial1_2(PlayerCore core)
        {
            this.core = core;
        }

        public void Enter()
        {
            core.isInvincible = true;
            core.attackAssist.CorrectionAttack();
            core.Animator.applyRootMotion = true;
            // アニメーションの遷移
            core.Animator.CrossFade("AttackSpecial1_2", 0.1f, 0, 0.3f);
        }

        public void Update()
        {
            AnimatorStateInfo stateInfo = core.Animator.GetCurrentAnimatorStateInfo(0);

            if (stateInfo.IsName("AttackSpecial1_2"))
            {
                if (stateInfo.normalizedTime >= 0.7)
                {
                    core.stateMachine.ChangeState(PlayerStateID.AttackSpecial1_3);
                }
            }
        }

        public void FixedUpdate() { }

        public void Exit()
        {
            core.Animator.applyRootMotion = false;
            core.isInvincible = false;
        }
    }
}

