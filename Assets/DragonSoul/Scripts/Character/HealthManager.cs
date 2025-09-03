using System;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    [SerializeField] float hp;  // HP
    
    const float minHP = 0;   // HP下限

    public float CurrentHP => hp;
    public float MaxHP { get; private set; }        // HP上限
    public bool IsDead => 0 >= hp;          // 死亡フラグ

    public Action<float, float> OnHPChanged;

    void Awake()
    {
        MaxHP = hp;
    }

    /// <summary>
    /// 回復処理
    /// </summary>
    /// <param name="healVal">回復量</param>
    public void Heal(float healVal)
    {
        hp = Mathf.Clamp(hp + healVal, minHP, MaxHP);

        OnHPChanged?.Invoke(hp, MaxHP);
    }

    /// <summary>
    /// ダメージ処理
    /// </summary>
    /// <param name="damageVal">ダメージ量</param>
    public void Damage(float damageVal)
    {
        hp = Mathf.Clamp(hp - damageVal, minHP, MaxHP);

        OnHPChanged?.Invoke(hp, MaxHP);
    }
}
