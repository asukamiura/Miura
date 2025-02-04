using UnityEditor.Rendering;
using UnityEngine;

namespace Player
{
    public class PlayerAttackSpecial1 : IState<PlayerStateID>
    {
        public PlayerStateID StateID => PlayerStateID.AttackSpecial1;
        PlayerCore core;
        InputReciver input => InputReciver.Instance;

        public PlayerAttackSpecial1(PlayerCore core)
        {
            this.core = core;
        }

        public void Enter()
        {
            core.isInvincible = true;
            core.attackAssist.CorrectionAttack();
            core.Animator.applyRootMotion = true;
            // アニメーションの遷移
            core.Animator.CrossFade("AttackSpecial1", 0.1f, 0, 0);
        }

        public void Update()
        {
            AnimatorStateInfo stateInfo = core.Animator.GetCurrentAnimatorStateInfo(0);
            // アニメーションが終わったらIdleStateに遷移
            if (stateInfo.normalizedTime >= 1)
            {
                core.stateMachine.ChangeState(PlayerStateID.Idle);
            }
        }

        public void FixedUpdate() { }

        public void Exit()
        {
            core.AttackEnd();
            core.Animator.applyRootMotion = false;
            core.isInvincible = false;
        }
    }
}

