using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HPGaugeView : MonoBehaviour
{
    [SerializeField] Image hpGauge;
    [SerializeField] Image smoothHPGauge;
    [SerializeField] float smoothSpeed = 1.0f;

    Coroutine smoothCoroutine;

    public void SetHP(float currentHP, float maxHP)
    {
        float targetFillAount = currentHP / maxHP;

        hpGauge.fillAmount = targetFillAount;
      

        if (targetFillAount >= smoothHPGauge.fillAmount)
        {
            if (smoothCoroutine != null)
            {
                StopCoroutine(smoothCoroutine);      
            }
            smoothHPGauge.fillAmount = targetFillAount;
        }
        else
        {
            if (smoothCoroutine != null)
            {
                StopCoroutine(smoothCoroutine);
            }

            if (targetFillAount <= 0)
            {
                smoothHPGauge.fillAmount = 0;
            }
            else
            {
                smoothCoroutine = StartCoroutine(Smooth(targetFillAount));            
            }
        }
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
