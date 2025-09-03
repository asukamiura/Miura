using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HPGaugeView : MonoBehaviour
{
    [SerializeField] Image hpGauge;
    [SerializeField] Image smoothHPGauge;
    [SerializeField] float smoothSpeed = 1.0f;

    public void SetHP(float currentHP, float maxHP)
    {
        float targetFillAount = currentHP / maxHP;

        hpGauge.fillAmount = targetFillAount;

        StartCoroutine(Smooth(targetFillAount));
    }

    IEnumerator Smooth(float targetFillAmount)
    {
        while (Mathf.Abs(smoothHPGauge.fillAmount - targetFillAmount) > 0.01f)
        {
            smoothHPGauge.fillAmount = Mathf.Lerp(smoothHPGauge.fillAmount, targetFillAmount, Time.deltaTime * smoothSpeed);
            yield return null;
        }

        smoothHPGauge.fillAmount = targetFillAmount;
    }
}
