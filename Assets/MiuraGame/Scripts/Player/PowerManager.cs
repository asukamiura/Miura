using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerManager : MonoBehaviour
{
    [SerializeField] private float defaultMoveSpeed = 5;
    [SerializeField] private float speedUpMultiplier = 1.3f;
    [SerializeField] private float attackPowerUpMultiplier = 1.5f;
    [SerializeField] private float powerUpDuration = 15;

    public float MoveSpeed { get; private set; }
    public bool InPowerUp { get; private set; } = false;

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
        MoveSpeed = defaultMoveSpeed;
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
        InPowerUp = true;
        StartCoroutine(ApplyPowerUp(powerUpDuration, attackPowerUpMultiplier, speedUpMultiplier));
        Debug.Log("パワーアップ");
    }

    /// <summary>
    /// パワーアップ処理
    /// </summary>
    /// <param name="duration">パワーアップの継続時間</param>
    /// <param name="attackPowerUpMultiplier">攻撃力アップ倍率</param>
    /// <param name="speedUpMultiplier">移動スピード倍率</param>
    private IEnumerator ApplyPowerUp(float duration, float attackPowerUpMultiplier, float speedUpMultiplier)
    {
        foreach (string key in baseAttackPower.Keys)
        {
            currentAttackPower[key] = baseAttackPower[key] * attackPowerUpMultiplier;
        }

        MoveSpeed = defaultMoveSpeed * speedUpMultiplier;

        yield return new WaitForSeconds(duration);

        ResetAttackPower();
        InPowerUp = false;
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

        MoveSpeed = defaultMoveSpeed;

        Debug.Log("パワーダウン");
    }
}
