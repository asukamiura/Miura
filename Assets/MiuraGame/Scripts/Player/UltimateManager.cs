using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltimateManager : MonoBehaviour
{
    [SerializeField] private float ultVal;  
    private float maxUltVal;  

    private void Start()
    {
        maxUltVal = ultVal;
    }

    public float ULTVal
    {
        get { return ultVal; }
        set
        {
            ultVal = Mathf.Clamp(ultVal, 0, maxUltVal);
        }
    }

    public void Heal(int healVal)
    {
        if (ultVal < maxUltVal)
        {
            ultVal += healVal;
        }
    }

    public void Damage(int damageVal)
    {
        if (ultVal > 0)
        {
            ultVal -= damageVal;
        }
    }
}
