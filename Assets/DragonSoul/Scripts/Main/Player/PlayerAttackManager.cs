using System;
using UnityEngine;

namespace Player
{
    public class PlayerAttackManager : MonoBehaviour
    {
        public event Action OnEnemykHit;

        [SerializeField] Transform playerTransform;
        [SerializeField] Transform attackTransform;
        [SerializeField] PlayerCore playerCore;
        [SerializeField] PowerManager powerManager;
        [SerializeField] UltimateManager ultimateManager;
        [SerializeField] ScoreManager scoreManager;
        [SerializeField] DamageUIGenerator damageUIGenerator;

        const float AttackRadius = 2f;
        const float ShowingTime = 1;

        void PerformAttack(Vector3 center, float radius)
        {
            Collider[] hitEnemies = Physics.OverlapSphere(center, radius);
            foreach (Collider enemy in hitEnemies)
            {
                float damageValue = 0;
                if (enemy.CompareTag("Enemy"))
                {
                    switch (playerCore.stateMachine.StateID)
                    {
                        case PlayerStateID.AttackNormal1:
                            damageValue = powerManager.AttackPower("Normal1");
                            ultimateManager.IncreaseGauge(1);
                            scoreManager.AddScore("AttackNormal1");
                            break;
                        case PlayerStateID.AttackNormal2:
                            damageValue = powerManager.AttackPower("Normal2");
                            ultimateManager.IncreaseGauge(2);
                            scoreManager.AddScore("AttackNormal2");
                            break;
                        case PlayerStateID.AttackNormal3:
                            damageValue = powerManager.AttackPower("Normal3");
                            ultimateManager.IncreaseGauge(3);
                            scoreManager.AddScore("AttackNormal3");
                            break;
                        case PlayerStateID.AttackSpecial1_1:
                            damageValue = powerManager.AttackPower("Special");
                            scoreManager.AddScore("AttackSpecial");
                            ultimateManager.IncreaseGauge(9);
                            break;
                        case PlayerStateID.AttackSpecial1_2:
                            damageValue = powerManager.AttackPower("Special");
                            scoreManager.AddScore("AttackSpecial");
                            ultimateManager.IncreaseGauge(9);
                            break;
                        case PlayerStateID.AttackSpecial1_3:
                            damageValue = powerManager.AttackPower("Special");
                            scoreManager.AddScore("AttackSpecial");
                            ultimateManager.IncreaseGauge(9);
                            break;
                        case PlayerStateID.AttackSpecial2:
                            damageValue = powerManager.AttackPower("Special");
                            scoreManager.AddScore("AttackSpecial");
                            ultimateManager.IncreaseGauge(9);
                            break;
                        case PlayerStateID.AttackCharge:
                            damageValue = powerManager.AttackPower("Charge");
                            scoreManager.AddScore("AttackCharge");
                            ultimateManager.IncreaseGauge(10);
                            break;
                        case PlayerStateID.AttackUltimate:
                            damageValue = powerManager.AttackPower("Ultimate");
                            scoreManager.AddScore("AttackUltimate");
                            break;
                    }

                    Vector3 closestPoint = enemy.ClosestPoint(playerTransform.position);

                    // 敵にダメージを与える
                    enemy.GetComponentInParent<HealthManager>().Damage(damageValue);

                    // ヒットエフェクトを生成
                    EffectGenerator.Instance.PlayEffect("HitEffect", closestPoint, transform.rotation, ShowingTime);

                    // ダメージUIを生成
                    damageUIGenerator.GenerateDamageUI(damageValue, closestPoint);

                    OnEnemykHit?.Invoke();
                }
            }
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
