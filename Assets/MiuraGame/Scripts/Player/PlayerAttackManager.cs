using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerAttackManager : MonoBehaviour
    {
        [SerializeField] private EffectGenerator effectGenerator;
        [SerializeField] private Transform attackTransform;
        [SerializeField] private PowerManager powerManager;

        private void PerformAttack(Vector3 center, float radius)
        {
            Debug.Log("Hit");
            Collider[] hitEnemies = Physics.OverlapSphere(center, radius);
            foreach (Collider enemy in hitEnemies)
            {
                if (enemy.CompareTag("Enemy"))
                {
                    enemy.GetComponentInParent<HealthManager>().Damage(powerManager.AttackPower("Normal1"));
                    effectGenerator.PlayEffect("HitEffect", enemy.ClosestPoint(transform.position), Quaternion.identity);
                }
            }
        }

        public void OnAttackHit()
        {
            PerformAttack(attackTransform.position, 2);
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackTransform.position, 2); // 攻撃範囲を表示
        }
    }
}
