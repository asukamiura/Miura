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
            core.isInvincible = true;
            core.attackAssist.CorrectionAttack();
            core.Animator.applyRootMotion = true;
            // アニメーションの遷移
            core.Animator.CrossFade("AttackSpecial1_1", 0.1f, 0, 0.3f);
        }

        public void Update()
        {
            AnimatorStateInfo stateInfo = core.Animator.GetCurrentAnimatorStateInfo(0);
            // アニメーションが終わったらIdleStateに遷移
            if (stateInfo.IsName("AttackSpecial1_1"))
            {
                if (stateInfo.normalizedTime >= 0.7)
                {
                    core.stateMachine.ChangeState(PlayerStateID.AttackSpecial1_2);
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

