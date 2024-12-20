using UnityEngine;

namespace Player
{
    public class AttackCorrectionManager : MonoBehaviour
    {
        private GameObject enemy;
        private float distance;
        private Vector3 direction;
        public bool isCorrectPosition = false;
        private bool isCorrectRotation = false;
        private const float MaxActiveDistance = 10;      // 攻撃補正を有効にする最大距離
        private const float MinActiveDistance = 2;      // 攻撃補正を有効にする最大距離
        private float correctionSpeed = 30;
        private PlayerCore playerCore;
        private Rigidbody rb;
        private InputReciver Input => InputReciver.Instance;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
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

        private void FixedUpdate()
        {
            if (distance <= MaxActiveDistance && distance >= MinActiveDistance && isCorrectPosition)
            {
                if (playerCore.stateMachine.StateID == PlayerStateID.AttackSpecial1
                    || playerCore.stateMachine.StateID == PlayerStateID.AttackSpecial2) { return; }

                transform.position = Vector3.MoveTowards(transform.position, enemy.transform.position, correctionSpeed * Time.deltaTime);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                isCorrectPosition = false;
                rb.velocity = Vector3.zero;
            }
        }

        public void CorrectionAttack()
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }
}
