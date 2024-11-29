using System.Collections;
using UnityEngine;

namespace Player
{
    public class JudgeJustDodge : MonoBehaviour
    {
        private PlayerCore core;
        private float currentTime = 0;
        private const int getJustPoints = 1;    // ジャスト回避成功時に得るジャストポイント量

        private void Start()
        {
            core = GetComponentInParent<PlayerCore>();
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
                    Debug.Log("Slow");
                    core.TimingUIShow("Slow");
                }
                else if (currentTime >= 0.1f && currentTime < 0.4)
                {
                    Debug.Log("Just");
                    core.TimingUIShow("Just");
                    core.justPointManager.AddJustPoints(getJustPoints);
                }
                else if (currentTime >= 0.4f && currentTime < 0.5f)
                {
                    Debug.Log("Fast");
                    core.TimingUIShow("Fast");
                }

                core.isJustDodge = true;
                Animator enemyAnimator = other.GetComponentInParent<Animator>();
                enemyAnimator.speed = 0.1f;
                core.Animator.speed = 0.3f;
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
        }
    }
}

