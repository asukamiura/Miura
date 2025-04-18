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

        void PerformAttack(Vector3 center, float radius)
        {
            Collider[] hitEnemies = Physics.OverlapSphere(center, radius);
            foreach (Collider enemy in hitEnemies)
            {
                if (enemy.CompareTag("Enemy"))
                {
                    switch (playerCore.stateMachine.StateID)
                    {
                        case PlayerStateID.AttackNormal:
                            ultimateManager.IncreaseGauge(1);
                            scoreManager.AddScore("AttackNormal1");
                            break;                      
                        case PlayerStateID.AttackSpecial1:
                            scoreManager.AddScore("AttackSpecial");
                            ultimateManager.IncreaseGauge(9);
                            break;
                        case PlayerStateID.AttackSpecial2:
                            scoreManager.AddScore("AttackSpecial");
                            ultimateManager.IncreaseGauge(9);                                                   
                            break;
                        case PlayerStateID.AttackUltimate:
                            scoreManager.AddScore("AttackUltimate");
                            break;
                    }

                    Vector3 closestPoint = enemy.ClosestPoint(playerTransform.position);

                    // 敵にダメージを与える
                    enemy.GetComponentInParent<HealthManager>().Damage(powerManager.GetAttackPower());

                    // ヒットエフェクトを生成
                    EffectManager.Instance.PlayEffect("HitEffect", closestPoint, transform.rotation);

                    // ダメージUIを生成
                    damageUIGenerator.GenerateDamageUI(powerManager.GetAttackPower(), closestPoint);

                    OnEnemykHit?.Invoke();
                }
            }
        }

        public void PerformAttackHit()
        {
            PerformAttack(attackTransform.position, AttackRadius);
        }

        //void OnDrawGizmos()
        //{
        //    Gizmos.color = Color.red;
        //    Gizmos.DrawWireSphere(attackTransform.position, AttackRadius); // 攻撃範囲を表示          
        //}
    }
}
