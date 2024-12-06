using UnityEngine;

namespace Player
{
    public class AttackCorrectionManager : MonoBehaviour
    {
        private GameObject enemy;
        private float distance;
        private Vector3 direction;
        private bool isCorrectPosition = false;
        private bool isCorrectRotation = false;
        private const float mixActiveDistance = 1.5f;   // 攻撃補正を有効にする最小距離
        private const float maxActiveDistance = 5;      // 攻撃補正を有効にする最大距離
        private float correctionSpeed = 10;
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
                distance = Vector3.Distance(transform.position, new Vector3(enemy.transform.position.x, transform.position.y, enemy.transform.position.z));
                direction = enemy.transform.position - transform.position;
            }

            if (distance >= mixActiveDistance && distance <= maxActiveDistance)
            {
                if (Input.AttackNormal || Input.AttackCharge)
                {
                    isCorrectRotation = true;

                    if (playerCore.stateMachine.StateID == PlayerStateID.AttackSpecial1
                        || playerCore.stateMachine.StateID == PlayerStateID.AttackSpecial2) { return; }

                    isCorrectPosition = true;
                }
            }
            else
            {
                isCorrectPosition = false;
                isCorrectRotation = false;
            }
        }

        private void FixedUpdate()
        {
            if (isCorrectPosition)
            {
                Vector3 targetPosition = Vector3.MoveTowards(transform.position, enemy.transform.position, correctionSpeed * Time.deltaTime);
                rb.MovePosition(targetPosition);
            }

            if (isCorrectRotation)
            {
                transform.rotation = Quaternion.LookRotation(direction);
            }
        }

        //public void AttackCorrectionStart()
        //{
        //    isCorrectPosition = true;
        //    isCorrectRotation = true;
        //}

        //public void AttackCorrectionEnd()
        //{
        //    isCorrectPosition = false;
        //    isCorrectRotation = false;
        //}
    }
}
