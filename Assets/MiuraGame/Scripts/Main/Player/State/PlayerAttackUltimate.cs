using UnityEngine;

namespace Player
{
    public class PlayerAttackUltimate : IState<PlayerStateID>
    {
        public PlayerStateID StateID => PlayerStateID.AttackUltimate;
        PlayerCore core;
        InputReciver Input => InputReciver.Instance;

        bool isEffective = false;   // ブロック演出中はtrue,それ以外はfalse

        const float DefaultFOV = 70;            // 通常の視野角
        const float TargetFOV = 50;             // 演出時の視野角
        const float SpreadSpeed = 2;            // 視野角を広げる速度
        const float NarrowSpeed = 10;           // 視野角を狭める速度
        const float PerformanceAnimationSpeed = 0.3f;
        const float DefaultAnimationSpeed = 1;
        const float EffectiveTime = 1;          // 演出の効果時間
        const float TranstionAttackSpecialNormalizedTime = 0.75f;
        const float TranstionIdleNormalizedTime = 0.75f;

        public PlayerAttackUltimate(PlayerCore core)
        {
            this.core = core;
        }

        public void Enter()
        {
            core.Animator.applyRootMotion = true;
            core.isInvincible = true;
            // アニメーションの遷移
            core.Animator.CrossFade("AttackUltimate", 0);

            // ブロック演出を開始
            isEffective = true;
            core.playerCameraController.StartChangeFOV(TargetFOV, NarrowSpeed);
            core.animationController.ChangeAllAnimationSpeed(PerformanceAnimationSpeed);
        }

        public void StateUpdate()
        {
            AnimatorStateInfo stateInfo = core.Animator.GetCurrentAnimatorStateInfo(0);
            // アニメーションが終わったらIdleStateに遷移
            if(stateInfo.normalizedTime >= 0.2)
            {
                if (isEffective)
                {
                    core.playerCameraController.StartChangeFOV(DefaultFOV, SpreadSpeed);
                    core.animationController.ChangeAllAnimationSpeed(DefaultAnimationSpeed);
                    isEffective = false;
                }
            }

            if (stateInfo.normalizedTime >= 0.8f)
            {
                core.stateMachine.ChangeState(PlayerStateID.Idle);
            }
        }

        public void StateFixedUpdate() { }

        public void Exit()
        {
            core.Animator.applyRootMotion = false;
            core.isInvincible = false;
        }
    }
}
