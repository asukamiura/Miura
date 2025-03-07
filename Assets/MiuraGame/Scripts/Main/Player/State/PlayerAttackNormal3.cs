using UnityEngine;

namespace Player
{
    public class PlayerAttackNormal3 : IState<PlayerStateID>
    {
        public PlayerStateID StateID => PlayerStateID.AttackNormal3;
        PlayerCore core;
        InputReciver Input => InputReciver.Instance;

        const float ChargeNormalizedTime = 0.1f;
        const float TransitionNormalizedTime = 1f;
        const float ChargeAnimationApeed = 0.3f;
        const float DefaultAnimationSpeed = 1.1f;

        public PlayerAttackNormal3(PlayerCore core)
        {
            this.core = core;
        }

        public void Enter()
        {
            core.Animator.applyRootMotion = true;
            core.attackAssist.CorrectionAttack();
            // アニメーションの遷移
            core.Animator.CrossFade("AttackNormal3", 0.1f);
        }

        public void StateUpdate()
        {
            AnimatorStateInfo stateInfo = core.Animator.GetCurrentAnimatorStateInfo(0);
            // アニメーションが終わったらIdleStateに遷移
            if (stateInfo.normalizedTime <= ChargeNormalizedTime)
            {
                core.Animator.speed = ChargeAnimationApeed;
            }
            else if (stateInfo.normalizedTime >= TransitionNormalizedTime)
            {
                core.stateMachine.ChangeState(PlayerStateID.Idle);
            }
            else
            {
                core.Animator.speed = DefaultAnimationSpeed;
            }
        }

        public void StateFixedUpdate() { }

        public void Exit()
        {
            core.Animator.applyRootMotion = false;
            core.Animator.speed = DefaultAnimationSpeed;
        }
    }
}
