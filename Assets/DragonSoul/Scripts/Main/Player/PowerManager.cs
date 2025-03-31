using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerManager : MonoBehaviour
{
    [SerializeField] float defaultMoveSpeed = 5;
    [SerializeField] float speedUpMultiplier = 1.3f;
    [SerializeField] float attackPowerUpMultiplier = 1.5f;
    [SerializeField] float powerUpDuration = 15;

    public float MoveSpeed { get; private set; }
    public bool InPowerUp { get; private set; } = false;
    public float AttackPower { get; set; } = 0;

    readonly Dictionary<string, float> defaultAttackPower = new Dictionary<string, float>
    {
        { "Normal1", 2 },
        { "Normal2", 4 },
        { "Normal3", 6 },
        { "Special1_1", 8 },
        { "Special1_2", 10 },
        { "Special1_3", 20 },
        { "Special2_1", 8 },
        { "Special2_2", 10 },
        { "Special2_3", 12 },
        { "Special2_4", 20 },
        { "Ultimate", 40 },
    };

    Dictionary<string, float> currentAttackPower = new Dictionary<string, float>();

    void Awake()
    {
        ResetAttackPower();
    }

    /// <summary>
    /// 攻撃力を設定
    /// </summary>
    /// <param name="attackName"></param>
    public void SetAttackPower(string attackName)
    {
        if (currentAttackPower.TryGetValue(attackName, out float power))
        {
            AttackPower = power;
        }
    }

    /// <summary>
    /// パワーアップ効果を適用
    /// </summary>
    public void ActionPowerUp()
    {
        InPowerUp = true;
        StartCoroutine(ApplyPowerUp());
    }

    // パワーアップ処理
    IEnumerator ApplyPowerUp()
    {
        foreach (string key in defaultAttackPower.Keys)
        {
            currentAttackPower[key] = defaultAttackPower[key] * attackPowerUpMultiplier;
        }

        MoveSpeed = defaultMoveSpeed * speedUpMultiplier;

        yield return new WaitForSeconds(powerUpDuration);

        ResetAttackPower();

        InPowerUp = false;
    }

    /// <summary>
    /// 攻撃力を初期値に戻す
    /// </summary>
    void ResetAttackPower()
    {
        foreach (string key in defaultAttackPower.Keys)
        {
            currentAttackPower[key] = defaultAttackPower[key];
        }

        MoveSpeed = defaultMoveSpeed;
    }
}
