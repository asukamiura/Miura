using UnityEngine;

namespace Player
{
    public class PlayerAttackSpecial1_3 : IState<PlayerStateID>
    {
        public PlayerStateID StateID => PlayerStateID.AttackSpecial1_3;
        PlayerCore core;
        InputReciver input => InputReciver.Instance;

        public PlayerAttackSpecial1_3(PlayerCore core)
        {
            this.core = core;
        }

        public void Enter()
        {
            core.IsInvincible = true;
            core.attackAssist.CorrectionAttack();
            core.Animator.applyRootMotion = true;
            // アニメーションの遷移
            core.Animator.CrossFade("AttackSpecial1_3", 0.1f, 0, 0.1f);

            core.powerManager.SetAttackPower("Special1_3");
        }

        public void Update()
        {          
            if (core.CurrentStateInfo.IsName("AttackSpecial1_3"))
            {
                if (core.CurrentStateInfo.normalizedTime >= 0.8)
                {
                    core.stateMachine.ChangeState(PlayerStateID.Idle);
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

