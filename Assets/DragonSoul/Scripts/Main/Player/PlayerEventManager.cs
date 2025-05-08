using System;
using UnityEngine;

public class PlayerEventManager : MonoBehaviour
{
    public event Action OnHeal;

    public event Action OnPowerUp;

    public event Action OnAttack;

    public event Action OnGuard;

    public event Action OnDash;

    public event Action OnUltimateEnter;
    public event Action OnUltimateExit;

    // 回復イベントの発火処理
    public void TriggerHeal() { OnHeal?.Invoke(); }

    // パワーアップイベントの発火処理
    public void TriggerPowerUp() { OnPowerUp?.Invoke(); }

    // 攻撃イベントの発火処理
    public void TriggerAttack() { OnAttack?.Invoke(); }

    // ガードイベントの発火処理
    public void TriggerGuard() { OnGuard?.Invoke(); }

    // 回避イベントの発火処理
    public void TriggerDash() { OnDash?.Invoke(); }

    // 必殺技イベントの発火処理
    public void TriggerUltimateEnter() { OnUltimateEnter?.Invoke(); }
    public void TriggerUltimateExit() { OnUltimateExit?.Invoke(); }
}
