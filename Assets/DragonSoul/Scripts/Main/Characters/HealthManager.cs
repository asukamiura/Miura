using System;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    float currentHP = 0;  // 現在のHP
    
    const float MinHP = 0;   // HP下限

    public float CurrentHP => currentHP;
    public float MaxHP { get; private set; }  // HP上限
    public bool IsDead => 0 >= currentHP;     // 死亡フラグ

    public Action<float, float> OnHPChanged;

    /// <summary>
    /// 最大HPを設定
    /// </summary>
    /// <param name="maxHP"></param>
    public void SetMaxHP(float maxHP)
    {
        MaxHP = maxHP;
        currentHP = maxHP;
        OnHPChanged?.Invoke(currentHP, MaxHP);
    }

    /// <summary>
    /// 回復処理
    /// </summary>
    /// <param name="healVal">回復量</param>
    public void RestoreHP(float healVal)
    {
        currentHP = Mathf.Clamp(currentHP + healVal, MinHP, MaxHP);

        OnHPChanged?.Invoke(currentHP, MaxHP);
    }

    /// <summary>
    /// ダメージ処理
    /// </summary>
    /// <param name="damageVal">ダメージ量</param>
    public void ReduceHP(float damageVal)
    {
        currentHP = Mathf.Clamp(currentHP - damageVal, MinHP, MaxHP);

        OnHPChanged?.Invoke(currentHP, MaxHP);
    }
}
