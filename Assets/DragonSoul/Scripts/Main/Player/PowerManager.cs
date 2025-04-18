using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerManager : MonoBehaviour
{
    [SerializeField] float defaultMoveSpeed = 5;
    [SerializeField] float speedUpMultiplier = 1.3f;
    [SerializeField] float defaultAttackPower = 1;
    [SerializeField] float powerUpMultiplier = 1.5f;
    [SerializeField] float powerUpDuration = 15;

    float currentAttackPower = 0;

    public float MoveSpeed { get; private set; }
    public bool InPowerUp { get; private set; } = false;

    AttackType currentAttackType = AttackType.None;

    readonly Dictionary<AttackType, float> defaultAttackMultiplier = new Dictionary<AttackType, float>
    {
        { AttackType.Normal1, 2 },
        { AttackType.Normal2, 4 },
        { AttackType.Normal3, 6 },
        { AttackType.Special1_1, 8 },
        { AttackType.Special1_2, 10 },
        { AttackType.Special1_3, 20 },
        { AttackType.Special2_1, 8 },
        { AttackType.Special2_2, 10 },
        { AttackType.Special2_3, 12 },
        { AttackType.Special2_4, 20 },
        { AttackType.Ultimate, 40 },
    };

    void Awake()
    {
        ResetAttackPower();
    }

    /// <summary>
    /// 攻撃力を設定
    /// </summary>
    /// <param name="attackType">攻撃タイプ</param>
    public void SetAttackType(AttackType attackType)
    {      
        currentAttackType = attackType;
    }

    /// <summary>
    /// 与えるダメージを取得
    /// </summary>
    /// <returns>与えるダメージ</returns>
    public float GetAttackPower()
    {
        if (defaultAttackMultiplier.TryGetValue(currentAttackType, out float multiplier))
        {
            return currentAttackPower * multiplier;
        }

        return 0;
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
        currentAttackPower *= powerUpMultiplier;

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
        MoveSpeed = defaultMoveSpeed;
        currentAttackPower = defaultAttackPower;
    }
}
