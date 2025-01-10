using UnityEngine;

namespace Player
{
    public class AttackCorrectionManager : MonoBehaviour
    {
        private GameObject enemy;
        private float distance;
        private Vector3 direction;
        private PlayerCore playerCore;
        private InputReciver Input => InputReciver.Instance;

        private void Start()
        {
            enemy = GameObject.FindGameObjectWithTag("Enemy");
            playerCore = GetComponent<PlayerCore>();
        }

        private void Update()
        {
            if(enemy != null)
            {
                // プレイヤーと敵の距離の計算
                distance = Vector3.Distance(transform.position, enemy.transform.position);

                // 敵の方向計算
                direction = (enemy.transform.position - transform.position).normalized;
            }
        }

        public void CorrectionAttack()
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }
}
