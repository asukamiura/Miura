using SoundSystem;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace Player
{
    public class PlayerDodge : IState<PlayerStateID>
    {
        public PlayerStateID StateID => PlayerStateID.Dodge;
        InputReciver Input => InputReciver.Instance;
        PlayerCore core;
        float currentTime = 0;      // 演出の効果時間計測用
        bool isEffective = false;   // ブロック演出中はtrue,それ以外はfalse
        bool isNextAttack = false;  // 特殊攻撃2を行う場合true,行わない場合false

        const float NormalizedTimeOffset = 0.3f;       
        const float PerformanceAnimationSpeed = 0.3f;
        const float DefaultAnimationSpeed = 1;
        const float EffectiveTime = 1;          // 演出の効果時間
        const float CameraBlendTime = 0.5f;
        const float TranstionAttackSpecialNormalizedTime = 0.75f;
        const float TranstionIdleNormalizedTime = 0.75f;
        const float KnockBackPower = 50;
        const float DecelerationRate = 0.9f; // 減速率

        public PlayerDodge(PlayerCore core)
        {
            this.core = core;
        }

        public void Enter()
        {
            // プレイヤーを無敵状態にする
            core.IsInvincible = true;

            core.Animator.CrossFade("Dodge", 0, 0, NormalizedTimeOffset);

            // プレイヤーをノックバックさせる
            Vector3 knockbackDir = (-core.transform.forward + -core.transform.right).normalized;
            core.Rb.velocity = knockbackDir * KnockBackPower;

            // ブロック演出を開始
            isEffective = true;
            CameraManager.Instance.ApplyImpulse();

            core.StartCoroutine(CameraManager.Instance.SwitchCamera(CameraBlendTime));

            CameraManager.Instance.EnabledRecentering();
            core.animationController.ChangeAllAnimationSpeed(PerformanceAnimationSpeed);

            EffectManager.Instance.PlayEffect("NovaLight", core.transform.position, Quaternion.Euler(-90,0,0));
            core.effectPlayer.ShowEffect("SpikyExplosion");
        }

        public void Update()
        {
            if (core.CurrentStateInfo.IsName("Dodge"))
            {
                // 次攻撃の入力があった場合、特殊攻撃2に遷移
                if (core.CurrentStateInfo.normalizedTime >= TranstionAttackSpecialNormalizedTime && isNextAttack)
                {
                    core.stateMachine.ChangeState(PlayerStateID.AttackSpecial1_1);
                }
                else if (core.CurrentStateInfo.normalizedTime >= TranstionIdleNormalizedTime && !isNextAttack)
                {
                    core.stateMachine.ChangeState(PlayerStateID.Idle);
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
                    core.animationController.ChangeAllAnimationSpeed(DefaultAnimationSpeed);
                    isEffective = false;
                }
             
                currentTime += Time.deltaTime;
            }
        }

        public void FixedUpdate() 
        {
            core.Rb.velocity *= DecelerationRate;
        }

        public void Exit()
        {
            core.IsJustDodge = false;
            core.IsInvincible = false;
            currentTime = 0;
            isNextAttack = false;
            CameraManager.Instance.DisabledRecentering();
            core.StartCoroutine(CameraManager.Instance.SwitchCamera(1));
        }
    }
}
