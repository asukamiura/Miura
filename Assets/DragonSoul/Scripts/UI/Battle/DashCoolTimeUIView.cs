using UnityEngine;
using UnityEngine.UI;

public class DashCoolTimeUIView : MonoBehaviour
{
    [SerializeField] Image coolTimeUI;

    public void SetCoolTime(float currentCoolTime, float maxCoolTime)
    {
        float targetFillAount = currentCoolTime / maxCoolTime;
        coolTimeUI.fillAmount = targetFillAount;
    }
}
