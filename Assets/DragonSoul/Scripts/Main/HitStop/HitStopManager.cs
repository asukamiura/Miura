using System.Collections;
using UnityEngine;

public class HitStopManager : MonoBehaviour
{
    [SerializeField] PlayerAttackBroadcaster attackBroadcaster;

    Coroutine playerStopCoroutine;

    public void PlayHitStop(Animator playerAnimator, Animator[] enemyAnimators, float stopDuration, float shakeForce)
    {
        // 既にヒットストップ中の場合、一度リセットして上書きする
        if (playerStopCoroutine != null)
        {
            StopCoroutine(playerStopCoroutine);
        }
        
        playerStopCoroutine = StartCoroutine(StopPlayerAnimation(playerAnimator, stopDuration));
        StartCoroutine(StopEnemyAnimation(enemyAnimators, stopDuration));

        CameraManager.Instance.ApplyImpulse(shakeForce, stopDuration);
    }

    // アニメーションを止める処理
    IEnumerator StopPlayerAnimation(Animator playerAnimator, float duration)
    {
        float playerAnimationSpeed = playerAnimator.speed;
        playerAnimator.speed = 0;

        yield return new WaitForSeconds(duration);

        playerAnimator.speed = playerAnimationSpeed;
    }

    // アニメーションを止める処理
    IEnumerator StopEnemyAnimation(Animator[] enemyAnimators, float duration)
    {
        float[] enemyAnimationSpeeds = new float[enemyAnimators.Length];

        for (int i = 0; enemyAnimators.Length > i; i++)
        {
            enemyAnimationSpeeds[i] = enemyAnimators[i].speed;
            enemyAnimators[i].speed = 0;
        }

        yield return new WaitForSeconds(duration);

        for (int i = 0; i < enemyAnimators.Length; i++)
        {
            enemyAnimators[i].speed = enemyAnimationSpeeds[i];
        }
    }
}
