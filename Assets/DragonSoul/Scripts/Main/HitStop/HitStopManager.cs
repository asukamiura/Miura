using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

public class HitStopManager : MonoBehaviour
{
    /// <summary>
    /// ヒットストップを開始
    /// </summary>
    /// <param name="playerAnimator">プレイヤーのアニメーター</param>
    /// <param name="enemyAnimators">攻撃の当たった敵のアニメーター</param>
    /// <param name="attackType">攻撃のタイプ</param>
    public void OnHitStop(Animator playerAnimator, Animator[] enemyAnimators, float stopDuration, CameraShakeType shakeType)
    {
        StartCoroutine(StopPlayerAnimation(playerAnimator, stopDuration));
        StartCoroutine(StopEnemyAnimation(enemyAnimators, stopDuration / 4));

        CameraManager.Instance.ApplyImpulse(shakeType, stopDuration);
    }

    public void OnHitStop(Animator playerAnimator, Animator[] enemyAnimators, float stopDuration, float shakeForce)
    {
        StartCoroutine(StopPlayerAnimation(playerAnimator, stopDuration));
        StartCoroutine(StopEnemyAnimation(enemyAnimators, stopDuration / 4));

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
