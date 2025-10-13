using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerAttack : MonoBehaviour
    {
        [SerializeField] AttackSequenceData attackNormalSequence;
        [SerializeField] AttackSequenceData attackSpecial1Sequence;
        [SerializeField] AttackSequenceData attackSpecial2Sequence;
        [SerializeField] AttackSequenceData ultimateSequence;
        [SerializeField] Animator playerAnimator;
        [SerializeField] Transform playerTransform;
        [SerializeField] Transform attackTransform;
        [SerializeField] PowerManager powerManager;
        [SerializeField] UltimateManager ultimateManager;
        [SerializeField] ScoreManager scoreManager;
        [SerializeField] DamageUIGenerator damageUIGenerator;
        [SerializeField] HitStopManager hitStopManager;

        int currentCombStep = 0;
        AttackSequenceData currentSequence;

        readonly Dictionary<PlayerAttackType, AttackSequenceData> attackSequenceDic = new ();

        const float AttackRadius = 2f;

        void Awake()
        {
            attackSequenceDic.Add(PlayerAttackType.Normal, attackNormalSequence);
            attackSequenceDic.Add(PlayerAttackType.Special1, attackSpecial1Sequence);
            attackSequenceDic.Add(PlayerAttackType.Special2, attackSpecial2Sequence);            
            attackSequenceDic.Add(PlayerAttackType.Ultimate, ultimateSequence);            
        }

        void ExcuteAttack(PlayerAttackData attackData)
        {
            Collider[] hitColliders = Physics.OverlapSphere(attackTransform.position, attackData.AttackRange);
            List<Animator> hitEnemyAnimators = new List<Animator>();

            foreach (Collider enemy in hitColliders)
            {
                if (enemy.CompareTag("Enemy"))
                {
                    // 現在の攻撃タイプを取得
                    //PlayerAttackType currentType = typeHolder.GetAttackType();

                    float damage = powerManager.CurrentAttackPower * attackData.AttackMultiplier;

                    // スコアを加算
                    scoreManager.AddScore(attackData.Score);

                    // 必殺技ゲージを溜める
                    ultimateManager.IncreaseGauge(attackData.UltAmount);

                    // 攻撃が当たった敵のコライダーをListに追加
                    hitEnemyAnimators.Add(enemy.GetComponentInParent<Animator>());

                    // 攻撃が当たった位置を取得
                    Vector3 closestPoint = enemy.ClosestPoint(playerTransform.position);

                    //// 敵にダメージを与える
                    enemy.GetComponentInParent<IEnemyDamageable>().TakeDamage(damage, attackData.CanFlinch);

                    // ヒットエフェクトを生成
                    EffectManager.Instance.PlayEffect("HitEffect", closestPoint, transform.rotation);

                    // ダメージUIを生成
                    damageUIGenerator.GenerateDamageUI(damage, closestPoint, powerManager.InPowerUp);
                }
            }

            // ヒットストップを実行
            hitStopManager.OnHitStop(playerAnimator, hitEnemyAnimators.ToArray(), attackData.HitStopDuration, attackData.CameraShakeForce);
        }

        public void PerformAttackHit()
        {
            var attackData = currentSequence.GetAttackData(currentCombStep);
            ExcuteAttack(attackData);

            if (currentCombStep < currentSequence.CombStepCount - 1)
            {
                currentCombStep++;
            }
        }

        public void SetCombAttack(PlayerAttackType attackType)
        {
            // Dictionaryからシーケンスデータを取得
            if (attackSequenceDic.TryGetValue(attackType, out var sequence))
            {
                currentSequence = sequence;
            }

            currentCombStep = 0;
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackTransform.position, AttackRadius); // 攻撃範囲を表示          
        }

    }
}
