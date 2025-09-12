using Player;
using System;
using System.Collections.Generic;
using UnityEngine;

public class UltimateManager : MonoBehaviour
{
    float ultVal = 0;

    const int minUltVal = 0;    // 必殺技ゲージの下限

    public int MaxUltVal { get; set; } = 100;   // 必殺技ゲージの上限
    public float UltVal => ultVal;

    public Action<float, float> OnGaugeValueChanged;

    readonly Dictionary<PlayerAttackType, float> increaseValueData = new Dictionary<PlayerAttackType, float>
    {
        {PlayerAttackType.Normal1, 1},
        {PlayerAttackType.Normal2, 2},
        {PlayerAttackType.Normal3, 3},
        {PlayerAttackType.Special1_1, 5},
        {PlayerAttackType.Special1_2, 5},
        {PlayerAttackType.Special1_3, 5},
        {PlayerAttackType.Special2_1, 5},
        {PlayerAttackType.Special2_2, 5},
        {PlayerAttackType.Special2_3, 5},
        {PlayerAttackType.Special2_4, 5},
        {PlayerAttackType.Ultimate, 0 },
    };

    /// <summary>
    /// 必殺技ゲージ増加処理
    /// </summary>
    /// <param name="increaseVal">増加させる量</param>
    public void IncreaseGauge(PlayerAttackType currentType)
    {
        ultVal = Mathf.Clamp(ultVal + increaseValueData[currentType], minUltVal, MaxUltVal);

        OnGaugeValueChanged?.Invoke(ultVal, MaxUltVal);
    }

    /// <summary>
    /// 必殺技ゲージ減少処理
    /// </summary>
    /// <param name="decreaseVal">減少させる量</param>
    public void DecreaseGauge(int decreaseVal)
    {
        ultVal = Mathf.Clamp(ultVal - decreaseVal, minUltVal, MaxUltVal);

        OnGaugeValueChanged?.Invoke(ultVal, MaxUltVal);
    }
}
