using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    [SerializeField] private float powerUpMultiplier = 1.5f;
    [SerializeField] private float powerUpDuration = 15;

    public bool inPowerUp { get; private set; } = false;

    private readonly Dictionary<string, float> baseAttackPower = new Dictionary<string, float>
    {
        { "Normal1", 2 },
        { "Normal2", 4 },
        { "Normal3", 6 },
        { "Special", 18 },
        { "Charge" , 20 },
        { "Ultimate", 40 },
    };

    private Dictionary<string, float> currentAttackPower = new Dictionary<string, float>();

    private void Start()
    {
        ResetAttackPower();
    }

    /// <summary>
    /// 現在の攻撃力を取得
    /// </summary>
    /// <param name="attackName">攻撃の名前</param>
    /// <returns>攻撃力</returns>
    public float AttackPower(string attackName)
    {
        if (currentAttackPower.TryGetValue(attackName, out float power))
        {
            return power;
        }
        return 0;
    }
    
    /// <summary>
    /// パワーアップ効果を適用
    /// </summary>
    public void ActionPowerUp()
    {
        inPowerUp = true;
        StartCoroutine(ApplyPowerUp(powerUpDuration, powerUpMultiplier));
        Debug.Log("パワーアップ");
    }

    /// <summary>
    /// パワーアップ処理
    /// </summary>
    /// <param name="duration">パワーアップの継続時間</param>
    /// <param name="multiplier">パワーアップ倍率</param>
    private IEnumerator ApplyPowerUp(float duration, float multiplier)
    {
        foreach (string key in baseAttackPower.Keys)
        {
            currentAttackPower[key] = baseAttackPower[key] * multiplier;
        }

        yield return new WaitForSeconds(duration);

        ResetAttackPower();
        inPowerUp = false;
    }

    /// <summary>
    /// 攻撃力を初期値に戻す
    /// </summary>
    private void ResetAttackPower()
    {
        foreach (string key in baseAttackPower.Keys)
        {
            currentAttackPower[key] = baseAttackPower[key];
        }
    }
}
