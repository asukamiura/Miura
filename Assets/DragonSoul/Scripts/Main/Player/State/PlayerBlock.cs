using UnityEngine;

namespace Player
{
    public class PlayerBlock : IState<PlayerStateID>
    {
        public PlayerStateID StateID => PlayerStateID.Block;
        InputReciver Input => InputReciver.Instance;
        PlayerCore core;
        float currentTime = 0;      // 演出の効果時間計測用
        bool isEffective = false;   // ブロック演出中はtrue,それ以外はfalse
        bool isNextAttack = false;  // 特殊攻撃2を行う場合true,行わない場合false

        const float DefaultFOV = 80;            // 通常の視野角
        const float TargetFOV = 50;             // 演出時の視野角
        const float DefaultDutch = 0;           // 通常のカメラの角度
        const float TargetDutch = 5;            // 演出時のカメラの角度
        const float PerformanceAnimationSpeed = 0.3f;
        const float DefaultAnimationSpeed = 1;
        const float EffectiveTime = 1;          // 演出の効果時間
        const float TranstionAttackSpecialNormalizedTime = 0.75f;
        const float TranstionIdleNormalizedTime = 1;
        const float KnockBackPower = 10;
        const float DecelerationRate = 0.99f; // 減速率

        public PlayerBlock(PlayerCore core)
        {
            this.core = core;
        }

        public void Enter()
        {
            // プレイヤーを無敵状態にする
            core.IsInvincible = true;

            core.Animator.CrossFade("Block", 0);

            // プレイヤーをノックバックさせる
            core.Rb.AddForce(-core.transform.forward * KnockBackPower, ForceMode.VelocityChange);

            // ブロック演出を開始
            isEffective = true;
            CameraManager.Instance.EnabledRecentering();
            CameraManager.Instance.PlayCameraEffect(targetFOV: TargetFOV, targetDutch: TargetDutch, 0.1f);
            CameraManager.Instance.ApplyImpulse();
            core.animationController.ChangeAllAnimationSpeed(PerformanceAnimationSpeed);
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

            if (isEffective)
            {
                // 効果時間が過ぎたら演出を終了
                if (currentTime >= EffectiveTime)
                {
                    CameraManager.Instance.PlayCameraEffect(targetFOV: DefaultFOV, targetDutch: DefaultDutch, 2);
                    CameraManager.Instance.DisabledRecentering();
                    core.animationController.ChangeAllAnimationSpeed(DefaultAnimationSpeed);
                    isEffective = false;
                }
                else
                {
                    currentTime += Time.deltaTime;
                }
            }
        }

        public void FixedUpdate()
        {
            core.Rb.velocity *= DecelerationRate;
        }

        public void Exit()
        {
            //core.Rb.velocity = Vector3.zero;
            core.IsJustGuard = false;
            core.IsInvincible = false;
            currentTime = 0;
            isNextAttack = false;
        }
    }
}
