using UnityEngine;

namespace Player
{
    public class PlayerAttackSpecial1_1 : IState<PlayerStateID>
    {
        public PlayerStateID StateID => PlayerStateID.AttackSpecial1_1;
        PlayerCore core;
        InputReciver input => InputReciver.Instance;

        public PlayerAttackSpecial1_1(PlayerCore core)
        {
            this.core = core;
        }

        public void Enter()
        {
            core.IsInvincible = true;
            core.attackAssist.CorrectionAttack();
            core.Animator.applyRootMotion = true;
            // アニメーションの遷移
            core.Animator.CrossFade("AttackSpecial1_1", 0.1f, 0, 0.3f);

            core.powerManager.SetAttackPower("Special1_1");            
        }

        public void Update()
        {            
            // アニメーションが終わったらIdleStateに遷移
            if (core.CurrentStateInfo.IsName("AttackSpecial1_1"))
            {
                if (core.CurrentStateInfo.normalizedTime >= 0.7)
                {
                    core.stateMachine.ChangeState(PlayerStateID.AttackSpecial1_2);
                }
            }
        }

        public void FixedUpdate() { }

        public void Exit()
        {
            core.Animator.applyRootMotion = false;
            core.IsInvincible = false;
        }
    }
}

