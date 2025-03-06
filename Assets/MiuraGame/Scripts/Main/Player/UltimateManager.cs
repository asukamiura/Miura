using UnityEngine;

public class UltimateManager : MonoBehaviour
{
    float ultVal = 0;

    public int MaxUltVal { get; set; } = 100;

    public float UltVal => ultVal;

    /// <summary>
    /// 必殺技ゲージ増加処理
    /// </summary>
    /// <param name="increaseVal">増加させる量</param>
    public void IncreaseGauge(int increaseVal)
    {
        ultVal = Mathf.Clamp(ultVal + increaseVal, 0, MaxUltVal);
    }

    /// <summary>
    /// 必殺技ゲージ減少処理
    /// </summary>
    /// <param name="decreaseVal">減少させる量</param>
    public void DecreaseGauge(int decreaseVal)
    {
        ultVal = Mathf.Clamp(ultVal - decreaseVal, 0, MaxUltVal);
    }
}
