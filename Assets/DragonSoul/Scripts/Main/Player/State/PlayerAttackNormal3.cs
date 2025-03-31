using System.Collections;
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
            core.playerEventManager.TriggerAttack();

            core.powerManager.SetAttackPower("Normal3");
        }

        public void Update()
        {
            // アニメーションが終わったらIdleStateに遷移
            if (core.CurrentStateInfo.normalizedTime <= ChargeNormalizedTime)
            {
                core.Animator.speed = ChargeAnimationApeed;
            }
            else if (core.CurrentStateInfo.normalizedTime >= TransitionNormalizedTime)
            {
                core.stateMachine.ChangeState(PlayerStateID.Idle);
            }
            else
            {
                core.Animator.speed = DefaultAnimationSpeed;
            }
        }

        public void FixedUpdate() { }

        public void Exit()
        {
            core.Animator.applyRootMotion = false;
            core.Animator.speed = DefaultAnimationSpeed;
        }
    }
}
