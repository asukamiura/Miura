using UnityEngine;

public class UltimateManager : MonoBehaviour
{
    private float ultVal = 0;
    public int maxUltVal = 100;

    public float ULTVal
    {
        get { return ultVal; }
        set
        {
            ultVal = Mathf.Clamp(ultVal, 0, maxUltVal);
        }
    }

    public void IncreaseGauge(int healVal)
    {
        if (ultVal < maxUltVal)
        {
            ultVal += healVal;
        }
    }

    public void DecreaseGauge(int damageVal)
    {
        if (ultVal > 0)
        {
            ultVal -= damageVal;
        }
    }
}
