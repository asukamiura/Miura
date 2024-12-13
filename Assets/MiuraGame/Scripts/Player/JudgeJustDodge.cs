using Enemy;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Player
{
    public class JudgeJustDodge : MonoBehaviour
    {
        private PlayerCore core;
        private GameObject enemy;
        private Animator enemyAnimator;
        private DragonUsurperCore dragonUsurperCore;
        [SerializeField] private List<GameObject> slowObjs = new List<GameObject>();
        private float currentTime = 0;
        private const int getJustPoints = 1;    // ジャスト回避成功時に得るジャストポイント量

        private void Start()
        {
            core = GetComponentInParent<PlayerCore>();
            enemy = GameObject.FindGameObjectWithTag("Enemy");
            enemyAnimator = enemy.GetComponent<Animator>();
        }

        private void Update()
        {
            if (core.judgeDodgeCollider.enabled == true)
            {
                currentTime += Time.deltaTime;
            }
            else
            {
                currentTime = 0;
            }

            if (currentTime >= 0.5f)
            {
                core.judgeDodgeCollider.enabled = false;
            }

        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("EnemyAttackCanDodge") && !core.isJustDodge)
            {
                if (currentTime > 0 && currentTime < 0.1f)
                {
                    Debug.Log("Late");
                    core.TimingUIShow("Late");
                }
                else if (currentTime >= 0.1f && currentTime < 0.4)
                {
                    Debug.Log("Just");
                    core.justDodgeCount++;
                    core.TimingUIShow("Just");
                    core.justPointManager.AddJustPoints(getJustPoints);
                }
                else if (currentTime >= 0.4f && currentTime < 0.5f)
                {
                    Debug.Log("Fast");
                    core.TimingUIShow("Fast");
                }

                core.isJustDodge = true;
                core.isInvincible = true;
               
                core.Animator.speed = 0.3f;
                enemyAnimator.speed = 0.3f;
              
                StartCoroutine(SlowTime(2, enemyAnimator));
            }
        }

        IEnumerator SlowTime(float time, Animator enemyAnimator)
        {
            yield return new WaitForSeconds(1);
            core.Animator.speed = 1;
            yield return new WaitForSeconds(time);
            enemyAnimator.speed = 1;

            core.judgeDodgeCollider.enabled = false;
            core.isJustDodge = false;
            core.isInvincible = false;
        }

        IEnumerator SlowTimeEnergyBall(float time, Rigidbody enemyRigidbody)
        {
            yield return new WaitForSeconds(1);
            core.Animator.speed = 1;
            yield return new WaitForSeconds(time);
            enemyRigidbody.velocity *= 2f;
            core.judgeDodgeCollider.enabled = false;
            core.isJustDodge = false;
            core.isInvincible = false;
        }
    }
}

