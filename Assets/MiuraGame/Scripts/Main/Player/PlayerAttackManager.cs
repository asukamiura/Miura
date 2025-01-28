using UnityEngine;

namespace Player
{
    public class PlayerAttackManager : MonoBehaviour
    {
        [SerializeField] EffectGenerator effectGenerator;
        [SerializeField] Transform attackTransform;
        [SerializeField] PowerManager powerManager;

        void PerformAttack(Vector3 center, float radius)
        {
            Debug.Log("Hit");
            Collider[] hitEnemies = Physics.OverlapSphere(center, radius);
            foreach (Collider enemy in hitEnemies)
            {
                if (enemy.CompareTag("Enemy"))
                {
                    enemy.GetComponentInParent<HealthManager>().Damage(powerManager.AttackPower("Normal1"));
                    effectGenerator.PlayEffect("HitEffect", enemy.ClosestPoint(transform.position), Quaternion.identity,1);
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
