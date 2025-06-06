using System.Collections;
using UnityEngine;

namespace Player
{
    public class PlayerBlock : IState<PlayerStateID>
    {
        public PlayerStateID StateID => PlayerStateID.Block;
        InputReciver Input => InputReciver.Instance;
        PlayerCore core;
        bool isNextAttack = false;  // 特殊攻撃2を行う場合true,行わない場合false

        const float DefaultFOV = 80;            // 通常の視野角
        const float TargetFOV = 50;             // 演出時の視野角
        const float DefaultDutch = 0;           // 通常のカメラの角度
        const float TargetDutch = 7;            // 演出時のカメラの角度
        const float PerformanceAnimationSpeed = 0;
        const float SlowAnimationSpeed = 0.5f;
        const float DefaultAnimationSpeed = 1;
        const float StopDuration = 0.25f; // 動きを止める時間
        const float SlowDuration = 0.3f; // 動きを遅くする時間
        const float TranstionAttackSpecialNormalizedTime = 0.75f;
        const float TranstionIdleNormalizedTime = 1;
        const float KnockBackPower = 20;
        const float DecelerationRate = 0.95f; // 減速率

        public PlayerBlock(PlayerCore core)
        {
            this.core = core;
        }

        public void Enter()
        {
            PostEffectManager.Instance.ChangePostEffect(ProfileNum.JustGuard, 0);

            // プレイヤーを無敵状態にする
            core.IsInvincible = true;

            core.Animator.CrossFade("Block", 0);

            // ブロック演出を開始
            CameraManager.Instance.EnabledRecentering();
            CameraManager.Instance.PlayCameraEffect(targetFOV: TargetFOV, targetDutch: TargetDutch, 0.1f);
            CameraManager.Instance.ApplyImpulse();

            core.effectPlayer.ShowEffect("NovaLight");

            core.StartCoroutine(StopMotion());
        }

        public void Update()
        {
            if (core.CurrentStateInfo.IsName("Block"))
            {
                // 次攻撃の入力があった場合、特殊攻撃2に遷移
                if (core.CurrentStateInfo.normalizedTime >= TranstionAttackSpecialNormalizedTime && isNextAttack)
                {
                    core.stateMachine.ChangeState(PlayerStateID.AttackSpecial2);
                }
                else if (core.CurrentStateInfo.normalizedTime >= TranstionIdleNormalizedTime && !isNextAttack)
                {
                    core.stateMachine.ChangeState(PlayerStateID.Locomotion);
                }

                if (Input.AttackNormal)
                {
                    isNextAttack = true;
                }
            }
        }

        public void FixedUpdate()
        {
            core.Rb.velocity *= DecelerationRate;
        }

        public void Exit()
        {
            core.Rb.velocity = Vector3.zero;
            core.IsJustGuard = false;
            core.IsInvincible = false;
            isNextAttack = false;
            PostEffectManager.Instance.ChangePostEffect(ProfileNum.Normal, 1);
        }

        IEnumerator StopMotion()
        {
            core.Animator.speed = PerformanceAnimationSpeed;
            SlowManager.Instance.ApplySlow(PerformanceAnimationSpeed);

            yield return new WaitForSeconds(StopDuration);

            core.Animator.speed = SlowAnimationSpeed;
            SlowManager.Instance.ApplySlow(SlowAnimationSpeed);

            // プレイヤーをノックバックさせる
            core.Rb.velocity = -core.transform.forward * KnockBackPower;

            CameraManager.Instance.PlayCameraEffect(targetFOV: DefaultFOV, targetDutch: DefaultDutch, 2);
            CameraManager.Instance.DisabledRecentering();

            yield return new WaitForSeconds(SlowDuration);

            core.Animator.speed = DefaultAnimationSpeed;
            SlowManager.Instance.ApplySlow(DefaultAnimationSpeed);
        }
    }
}
