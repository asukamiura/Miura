using System;
using UnityEngine;

public class DashCooldownManager : MonoBehaviour
{
    float baseCoolTime;          // クールタイム初期値
    float currentCoolTime = 0;   // 現在のクールタイム
    bool canDash = true;         // trueはダッシュ可能、falseはダッシュ不可能

    public bool CanDash => canDash;
    public Action<float,float> OnCooldown;

    void Update()
    {
        if (!canDash && currentCoolTime > 0)
        {
            currentCoolTime -= Time.deltaTime;
            OnCooldown?.Invoke(currentCoolTime, baseCoolTime);

            if (currentCoolTime <= 0)
            {
                currentCoolTime = 0;
                canDash = true;
            }
        }      
    }

    // クールタイムをセット
    public void SetCoolTime(float coolTime)
    {
        baseCoolTime = coolTime;
        OnCooldown?.Invoke(currentCoolTime, baseCoolTime);
    }

    // クールタイムのカウントを開始
    public void StartCooldown()
    {
        currentCoolTime = baseCoolTime;
        canDash = false;
    }
}
