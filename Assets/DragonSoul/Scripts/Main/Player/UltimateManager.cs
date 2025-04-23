using Player;
using System.Collections.Generic;
using UnityEngine;

public class UltimateManager : MonoBehaviour
{
    float ultVal = 0;

    const int minUltVal = 0;    // 必殺技ゲージの下限

    public int MaxUltVal { get; set; } = 100;   // 必殺技ゲージの上限
    public float UltVal => ultVal;

    readonly Dictionary<AttackType, float> increaseValueData = new Dictionary<AttackType, float>
    {
        {AttackType.Normal1, 1},
        {AttackType.Normal2, 2},
        {AttackType.Normal3, 3},
        {AttackType.Special1_1, 5},
        {AttackType.Special1_2, 5},
        {AttackType.Special1_3, 5},
        {AttackType.Special2_1, 5},
        {AttackType.Special2_2, 5},
        {AttackType.Special2_3, 5},
        {AttackType.Special2_4, 5},
        {AttackType.Ultimate, 0 },
    };

    /// <summary>
    /// 必殺技ゲージ増加処理
    /// </summary>
    /// <param name="increaseVal">増加させる量</param>
    public void IncreaseGauge(AttackType currentType)
    {
        ultVal = Mathf.Clamp(ultVal + increaseValueData[currentType], minUltVal, MaxUltVal);
    }

    /// <summary>
    /// 必殺技ゲージ減少処理
    /// </summary>
    /// <param name="decreaseVal">減少させる量</param>
    public void DecreaseGauge(int decreaseVal)
    {
        ultVal = Mathf.Clamp(ultVal - decreaseVal, minUltVal, MaxUltVal);
    }
}
