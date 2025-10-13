using System.Collections;
using UnityEngine;

public class PowerManager : MonoBehaviour
{
    float baseAttackPower = 0;
    float basePowerUpMultiplier = 0;
    float powerUpDuration = 20;

    float currentAttackPower = 0;

    public bool InPowerUp { get; private set; } = false;
    public float CurrentAttackPower => currentAttackPower;  //現在の攻撃力

    void Awake()
    {
        ResetAttackPower();
    }

    public void SetParameter(float attackPower, float multiplier, float duration)
    {
        baseAttackPower = attackPower;
        basePowerUpMultiplier = multiplier;
        powerUpDuration = duration;
        currentAttackPower = baseAttackPower;
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
        currentAttackPower *= multiplier;

        yield return new WaitForSeconds(duration);

        ResetAttackPower();

        InPowerUp = false;
    }

    /// <summary>
    /// 攻撃力を初期値に戻す
    /// </summary>
    void ResetAttackPower()
    {
        currentAttackPower = baseAttackPower;
    }
}
