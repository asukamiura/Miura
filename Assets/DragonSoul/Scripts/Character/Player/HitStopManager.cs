using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

public class HitStopManager : MonoBehaviour
{
    class HitStopConfig
    {
        public readonly float stopDuration = 0;
        public readonly float impulseForce = 0;

        public HitStopConfig(float stopDuration, float impulseForce)
        {
            this.stopDuration = stopDuration;
            this.impulseForce = impulseForce;
        }
    }

    readonly Dictionary<AttackType, HitStopConfig> hitStopData = new Dictionary<AttackType, HitStopConfig>
    { 
        {AttackType.Normal1, new HitStopConfig(0.03f, 0f) },
        {AttackType.Normal2, new HitStopConfig(0.03f, 0f) },
        {AttackType.Normal3, new HitStopConfig(0.03f, 0f) },
        {AttackType.Special1_1, new HitStopConfig(0.08f, 0f) },
        {AttackType.Special1_2, new HitStopConfig(0.08f, 0.3f) },
        {AttackType.Special1_3, new HitStopConfig(0.1f, 1f) },
        {AttackType.Special2_1, new HitStopConfig(0.05f, 0.5f) },
        {AttackType.Special2_2, new HitStopConfig(0.03f, 0.1f) },
        {AttackType.Special2_3, new HitStopConfig(0.03f, 0.1f) },
        {AttackType.Special2_4, new HitStopConfig(0.3f, 1.5f) },
        {AttackType.Ultimate, new HitStopConfig(0.2f, 2f) },      
    };

    /// <summary>
    /// ヒットストップを開始
    /// </summary>
    /// <param name="playerAnimator">プレイヤーのアニメーター</param>
    /// <param name="enemyAnimators">攻撃の当たった敵のアニメーター</param>
    /// <param name="attackType">攻撃のタイプ</param>
    public void OnHitStop(Animator playerAnimator, Animator[] enemyAnimators, AttackType attackType)
    {
        StartCoroutine(StopPlayerAnimation(playerAnimator, hitStopData[attackType].stopDuration));
        StartCoroutine(StopEnemyAnimation(enemyAnimators, hitStopData[attackType].stopDuration / 4));

        CameraManager.Instance.ApplyImpulse(hitStopData[attackType].impulseForce, hitStopData[attackType].stopDuration);
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
