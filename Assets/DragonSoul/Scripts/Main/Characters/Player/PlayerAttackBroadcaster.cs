using System;
using UnityEngine;

public class PlayerAttackBroadcaster : MonoBehaviour
{
    [SerializeField] IAttackProcessor[] attackProcessors;

    public Action<HitInfo> OnHitNotified;

    void OnEnable()
    {
        attackProcessors = GetComponentsInChildren<IAttackProcessor>();

        if (attackProcessors.Length == 0) { return; }

        foreach (var processor in attackProcessors)
        {
            processor.OnAttackHit += HandleTargetHit;
        }
    }

    void OnDisable()
    {
        if (attackProcessors.Length == 0) { return; }

        foreach (var processor in attackProcessors)
        {
            processor.OnAttackHit -= HandleTargetHit;
        }
    }

    void HandleTargetHit(HitInfo hitInfo)
    {
        OnHitNotified?.Invoke(hitInfo);
    }
}
