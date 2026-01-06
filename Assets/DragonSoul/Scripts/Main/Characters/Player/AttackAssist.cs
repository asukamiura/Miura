using UnityEngine;

namespace Player
{
    public class AttackAssist : MonoBehaviour
    {
        [SerializeField] Rigidbody rb;
        [SerializeField] float assistDistance = 8;
        [SerializeField] float assistPower = 80;
        [SerializeField] float stopDistance = 1;

        bool isAssisting = false;   // 攻撃アシストが有効な場合true,無効の場合false
        Collider targetCollider;
        float targetDistance = 0;

        void FixedUpdate()
        {
            if (!isAssisting) { return; }

            Vector3 closestTarget = targetCollider.ClosestPoint(rb.position);
            Vector3 playerPos = rb.position;
            Vector3 targetPos = new Vector3(closestTarget.x, 0, closestTarget.z);

            Vector3 direction = (targetPos - playerPos).normalized;

            rb.velocity = direction * assistPower * targetDistance / assistDistance;

            if (Vector3.Distance(playerPos, targetPos) < stopDistance)
            {
                rb.velocity = Vector3.zero;

                isAssisting = false;
            }
        }

        void OnDrawGizmos()
        {
            if (rb == null) { return; }
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(rb.position, assistDistance); // アシスト範囲を表示                                                             
        }

        /// <summary>
        /// ターゲットのコライダー、距離、方向を取得
        /// </summary>
        /// <returns>コライダー、距離、方向</returns>
        (Collider collider, float distance, Vector3 direction) GetClosestTarget()
        {
            // アシスト範囲にあるコライダーをすべて取得
            Collider[] hitColliders = Physics.OverlapSphere(rb.position, assistDistance);

            float closestDistance = float.MaxValue;
            Collider closestCollider = null;

            // 一番近い敵のコライダーとその距離を取得
            foreach (var collider in hitColliders)
            {
                if (!collider.CompareTag("Enemy")) continue;

                // 敵のコライダーの中で一番プレイヤーに近い点との距離
                float distance = Vector3.Distance(rb.position, collider.ClosestPoint(rb.position));

                // 距離を比べる
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestCollider = collider;
                }
            }

            if (closestCollider == null) { return (null, 0, Vector3.zero); }

            Vector3 closestPoint = closestCollider.ClosestPoint(rb.position);

            // ターゲットの方向
            Vector3 targetDirection = (new Vector3(closestPoint.x, 0, closestPoint.z) - rb.position).normalized;

            return (closestCollider, closestDistance, targetDirection);
        }

        /// <summary>
        /// 攻撃アシストを有効
        /// </summary>
        public void OnAssist()
        {
            var (collider, distance, direction) = GetClosestTarget();

            // アシスト範囲内にターゲットがいなかった場合
            if (collider == null)
            {
                // 入力があった場合、入力方向を向く
                if (InputReciver.Instance.Move != Vector2.zero)
                {
                    Quaternion cameraRotation = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);
                    Vector3 moveDirection = cameraRotation * new Vector3(InputReciver.Instance.Move.x, 0, InputReciver.Instance.Move.y);
                    Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
                    transform.rotation = targetRotation;
                }              

                return;
            }

            targetCollider = collider;

            targetDistance = distance;

            isAssisting = true;

            transform.rotation = Quaternion.LookRotation(direction);
        }

        /// <summary>
        /// 攻撃アシストを無効
        /// </summary>
        public void StopAssist()
        {
            isAssisting = false;
        }

        public void CorrectionAttack()
        {
            var (_, _, direction) = GetClosestTarget();
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }
}
