using System;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerAttackManager : MonoBehaviour
    {
        public event Action OnEnemyHit;

        [SerializeField] Animator playerAnimator;
        [SerializeField] Transform playerTransform;
        [SerializeField] Transform attackTransform;
        [SerializeField] AttackTypeHolder attackController;
        [SerializeField] PowerManager powerManager;
        [SerializeField] UltimateManager ultimateManager;
        [SerializeField] ScoreManager scoreManager;
        [SerializeField] DamageUIGenerator damageUIGenerator;
        [SerializeField] HitStopManager hitStopManager;

        const float AttackRadius = 2f;

        void PerformAttack(Vector3 center, float radius)
        {
            Collider[] hitColliders = Physics.OverlapSphere(center, radius);
            List<Animator> hitEnemyAnimators = new List<Animator>();

            foreach (Collider enemy in hitColliders)
            {
                if (enemy.CompareTag("Enemy"))
                {
                    // 現在の攻撃タイプを取得
                    AttackType currentType = attackController.GetAttackType();

                    // スコアを加算
                    scoreManager.AddScore(currentType.ToString());

                    // 必殺技ゲージを溜める
                    ultimateManager.IncreaseGauge(currentType);

                    // 攻撃が当たった敵のコライダーをListに追加
                    hitEnemyAnimators.Add(enemy.GetComponentInParent<Animator>());

                    // 攻撃が当たった位置を取得
                    Vector3 closestPoint = enemy.ClosestPoint(playerTransform.position);

                    // 敵にダメージを与える
                    enemy.GetComponentInParent<HealthManager>().Damage(powerManager.GetAttackPower(currentType));
                    
                    // ヒットエフェクトを生成
                    EffectManager.Instance.PlayEffect("HitEffect", closestPoint, transform.rotation);

                    // ダメージUIを生成
                    damageUIGenerator.GenerateDamageUI(powerManager.GetAttackPower(currentType), closestPoint, powerManager.InPowerUp);

                    OnEnemyHit?.Invoke();
                }
            }

            // ヒットストップを実行
            hitStopManager.OnHitStop(playerAnimator, hitEnemyAnimators.ToArray(), attackController.GetAttackType());
        }

        public void PerformAttackHit()
        {
            PerformAttack(attackTransform.position, AttackRadius);
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackTransform.position, AttackRadius); // 攻撃範囲を表示          
        }
    }
}
