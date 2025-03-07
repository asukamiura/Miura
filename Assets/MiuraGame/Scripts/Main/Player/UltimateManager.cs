using UnityEngine;

public class UltimateManager : MonoBehaviour
{
    float ultVal = 0;

    const int minUltVal = 0;    // 必殺技ゲージの下限

    public int MaxUltVal { get; set; } = 100;   // 必殺技ゲージの上限
    public float UltVal => ultVal;

    /// <summary>
    /// 必殺技ゲージ増加処理
    /// </summary>
    /// <param name="increaseVal">増加させる量</param>
    public void IncreaseGauge(int increaseVal)
    {
        ultVal = Mathf.Clamp(ultVal + increaseVal, minUltVal, MaxUltVal);
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
