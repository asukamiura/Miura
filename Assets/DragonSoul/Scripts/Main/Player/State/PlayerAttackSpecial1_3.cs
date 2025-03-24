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
            core.isInvincible = true;
            core.attackAssist.CorrectionAttack();
            core.Animator.applyRootMotion = true;
            // アニメーションの遷移
            core.Animator.CrossFade("AttackSpecial1_3", 0.1f, 0, 0.1f);
        }

        public void Update()
        {
            AnimatorStateInfo stateInfo = core.Animator.GetCurrentAnimatorStateInfo(0);

            if (stateInfo.IsName("AttackSpecial1_3"))
            {
                if (stateInfo.normalizedTime >= 0.8)
                {
                    core.stateMachine.ChangeState(PlayerStateID.Idle);
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

