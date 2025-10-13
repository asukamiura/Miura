using System;
using UnityEngine;

public class UltimateManager : MonoBehaviour
{
    float ultVal = 0;
    const float MinUltVal = 0;    // 必殺技ゲージの下限
    public float MaxUltVal { get; private set; } = 100;   // 必殺技ゲージの上限
    public float UltVal => ultVal;

    public Action<float, float> OnGaugeValueChanged;

    /// <summary>
    /// 必殺技ゲージ増加処理
    /// </summary>
    /// <param name="amount">増加させる量</param>
    public void IncreaseGauge(float amount)
    {
        ultVal = Mathf.Clamp(ultVal + amount, MinUltVal, MaxUltVal);

        OnGaugeValueChanged?.Invoke(ultVal, MaxUltVal);
    }

    /// <summary>
    /// 必殺技ゲージ減少処理
    /// </summary>
    /// <param name="amount">減少させる量</param>
    public void DecreaseGauge(float amount)
    {
        ultVal = Mathf.Clamp(ultVal - amount, MinUltVal, MaxUltVal);

        OnGaugeValueChanged?.Invoke(ultVal, MaxUltVal);
    }
}
