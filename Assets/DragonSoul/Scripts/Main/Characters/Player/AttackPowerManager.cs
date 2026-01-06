using System;
using System.Collections;
using UnityEngine;

public class AttackPowerManager : MonoBehaviour
{
    float baseAttackPower = 0;
    float basePowerUpMultiplier = 0;
    float powerUpDuration = 0;

    public Action OnPowerUpEnd;
    public bool InPowerUp { get; private set; } = false;
    public float CurrentAttackPower { get; private set; }  //現在の攻撃力

    public void SetParameter(float attackPower, float multiplier, float duration)
    {
        baseAttackPower = attackPower;
        basePowerUpMultiplier = multiplier;
        powerUpDuration = duration;
        CurrentAttackPower = baseAttackPower;
    }

    /// <summary>
    /// パワーアップ効果を適用
    /// </summary>
    public void ActionPowerUp()
    {
        InPowerUp = true;
        StartCoroutine(ApplyPowerUp(basePowerUpMultiplier, powerUpDuration));
    }

    // パワーアップ処理
    IEnumerator ApplyPowerUp(float multiplier, float duration)
    {
        // 攻撃力を強化
        CurrentAttackPower *= multiplier;

        // 強化時間が経過するまで待つ
        yield return new WaitForSeconds(duration);

        // 攻撃力を初期値に戻す
        CurrentAttackPower = baseAttackPower;
        OnPowerUpEnd?.Invoke();
        InPowerUp = false;
    }
}
