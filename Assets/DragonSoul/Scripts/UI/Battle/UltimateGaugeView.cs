using UnityEngine;
using UnityEngine.UI;

public class UltimateGaugeView : MonoBehaviour
{
    [SerializeField] Image ultimateGauge;

    public void SetGaugeValue(float value, float maxValue)
    {
        ultimateGauge.fillAmount = value / maxValue;
    }
}
