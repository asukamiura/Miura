using System;
using UnityEngine;

namespace Player
{
    public class PlayerAttackSpecial1_2 : IState<PlayerStateID>
    {
        public PlayerStateID StateID => PlayerStateID.AttackSpecial1_2;
        PlayerCore core;
        InputReciver input => InputReciver.Instance;

        bool isEffective = false;

        public PlayerAttackSpecial1_2(PlayerCore core)
        {
            this.core = core;
        }

        public void Enter()
        {
            core.IsInvincible = true;
            core.attackAssist.CorrectionAttack();
            core.Animator.applyRootMotion = true;
            // アニメーションの遷移
            core.Animator.CrossFade("AttackSpecial1_2", 0.1f, 0, 0.3f);

            core.powerManager.SetAttackPower("Special1_2");
        }

        public void Update()
        {           
            if (core.CurrentStateInfo.IsName("AttackSpecial1_2"))
            {
                if (!isEffective && core.CurrentStateInfo.normalizedTime > 0.6)
                {
                    isEffective = true;
                    EffectManager.Instance.PlayEffect("SpikeEffect", core.transform.position, Quaternion.identity);

                }
                else if (core.CurrentStateInfo.normalizedTime >= 0.7)
                {
                    core.stateMachine.ChangeState(PlayerStateID.AttackSpecial1_3);
                }
            }
        }

        public void FixedUpdate() { }

        public void Exit()
        {
            isEffective = false;
            core.Animator.applyRootMotion = false;
            core.IsInvincible = false;
        }
    }
}

