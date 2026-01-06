using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerAttackControllerBase : MonoBehaviour, IAttackProcessor
{
    [SerializeField] protected Transform attackPoint;
    [SerializeField] protected HitStopManager hitStopManager;
    [SerializeField] protected LayerMask targetLayer;

    public Action<PlayerAttackData> OnAttackExecuted;   // 攻撃が実行されたことを通知するイベント
    public Action OnHitDetectionPerformed;              // 攻撃の当たり判定を有効にするのを通知するイベント
    public event Action<HitInfo> OnAttackHit;           // 攻撃が命中したことを通知するイベント

    public virtual void ExecuteAttack(PlayerAttackData attackData, float attackPower, bool inPowerUp, Transform playerCenterTransform, Animator playerAnimator)
    {
        // 攻撃が実行されたことを通知
        OnAttackExecuted?.Invoke(attackData);

        Collider[] hitColliders = Physics.OverlapSphere(attackPoint.position, attackData.AttackRange, targetLayer);
        List<Animator> hitEnemyAnimators = new List<Animator>();
        Dictionary<IEnemyDamageable, Collider> nearestColliderMap = new Dictionary<IEnemyDamageable, Collider>();

        foreach (Collider collider in hitColliders)
        {
            // 親からダメージ対象を取得
            var damageable = collider.GetComponentInParent<IEnemyDamageable>();

            if (damageable == null) { continue; }

            // プレイヤーから見た、このコライダーの一番近い点
            Vector3 closestPoint = collider.ClosestPointOnBounds(playerCenterTransform.position);
            // プレイヤーから一番近い点までの距離
            float distance = Vector3.Distance(playerCenterTransform.position, closestPoint);

            // 既にこのコライダーを持つ敵が登録されているか？
            if (nearestColliderMap.TryGetValue(damageable, out var registered))
            {
                // 既に登録されているコライダーの一番近い点との距離を比較
                Vector3 registeredPoint = registered.ClosestPointOnBounds(playerCenterTransform.position);
                float registeredDistance = Vector3.Distance(playerCenterTransform.position, registeredPoint);

                // より近いならコライダー差し替え
                if (distance < registeredDistance)
                {
                    nearestColliderMap[damageable] = collider;
                }
            }
            else
            {
                // 初めて当たった敵はそのまま登録
                nearestColliderMap.Add(damageable, collider);
            }
        }

        foreach (var pair in nearestColliderMap)
        {
            var damageable = pair.Key;
            var collider = pair.Value;

            // 攻撃が当たった位置を取得
            Vector3 closestPoint = collider.ClosestPointOnBounds(playerCenterTransform.position);

            // 与えるダメージを計算
            float damage = attackPower * attackData.AttackMultiplier;

            // 敵にダメージを与える
            damageable.TakeDamage(damage, attackData.CanFlinch);

            HitInfo hitInfo = new HitInfo
            {
                hitPoint = closestPoint,
                attackData = attackData,
                damage = damage,
                inPowerUp = inPowerUp,
            };

            // 攻撃が敵に命中したことを通知
            OnAttackHit?.Invoke(hitInfo);

            // スコアを加算
            ScoreManager.Instance.AddScore(attackData.Score);

            // 攻撃が当たった敵のアニメーターをListに追加
            hitEnemyAnimators.Add(collider.GetComponentInParent<Animator>());

            // ヒットエフェクトを生成
            EffectManager.Instance.PlayEffect("HitEffect", closestPoint, transform.rotation);
        }

        // ヒットストップを実行
        hitStopManager.PlayHitStop(playerAnimator, hitEnemyAnimators.ToArray(), attackData.HitStopDuration, attackData.CameraShakeForce);
    }
}
