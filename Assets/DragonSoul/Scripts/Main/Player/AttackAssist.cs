using UnityEngine;

namespace Player
{
    public class AttackAssist : MonoBehaviour, IMatchTarget
    {
        [SerializeField] GameObject target;
        [SerializeField] Collider[] targetColliders;

        Animator animator;
        Collider targetCollider;
        InputReciver Input => InputReciver.Instance;
        Vector3 direction;

        void Start()
        {
            animator = GetComponent<Animator>();
            target = GameObject.FindWithTag("Enemy");
            targetColliders = target.GetComponentsInChildren<Collider>(false);
     
            animator.keepAnimatorStateOnDisable = true;

            foreach (var smb in animator.GetBehaviours<MatchPositionSMB>())
            {
                smb.target = this;
            }
        }

        void Update()
        {
            if (target != null)
            {
                // 敵の方向計算
                direction = (target.transform.position - transform.position).normalized;
            }

            UpdateClosestTarget();
        }

        public void CorrectionAttack()
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }

        // 攻撃アシストのターゲット更新
        public void UpdateClosestTarget()
        {
            float closestDistance = float.MaxValue;
            Collider closestCollider = null;

            foreach (var collider in targetColliders)
            {
                float distance = Vector3.Distance(transform.position, collider.transform.position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestCollider = collider;
                }
            }

            targetCollider = closestCollider;
        }

        public Vector3 TargetPosition => targetCollider.ClosestPoint(transform.position);
    }
}
