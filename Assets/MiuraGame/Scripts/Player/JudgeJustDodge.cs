using Enemy;
using SoundSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class JudgeJustDodge : MonoBehaviour
    {
        [SerializeField] private List<GameObject> slowObjs = new List<GameObject>();

        private PlayerCore core;
        private GameObject enemy;
        private Animator enemyAnimator;
        private DragonUsurperCore dragonUsurperCore;
        private float currentTime = 0;

        private const int GetJustPoints = 1;    // ジャスト回避成功時に得るジャストポイント量
        private const float PlayerSlowTime = 1;
        private const float EnemySlowTime = 3;
        private const float DefaultAnimationSpeed = 1;
        private const float SlowAnimationSpeed = 0.3f;

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
                    core.justPointManager.AddJustPoints(GetJustPoints);
                }
                else if (currentTime >= 0.4f && currentTime < 0.5f)
                {
                    Debug.Log("Fast");
                    core.TimingUIShow("Fast");
                }

                StartCoroutine(SlowTime(enemyAnimator));
            }
        }

        IEnumerator SlowTime(Animator enemyAnimator)
        {
            core.isJustDodge = true;
            core.isInvincible = true;

            SoundManager.Instance.PlaySe("SlowTime");

            core.Animator.speed = SlowAnimationSpeed;
            enemyAnimator.speed = SlowAnimationSpeed;

            yield return new WaitForSeconds(PlayerSlowTime);
           
            core.Animator.speed = DefaultAnimationSpeed;

            yield return new WaitForSeconds(EnemySlowTime);

            enemyAnimator.speed = DefaultAnimationSpeed;

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

