using System.Collections.Generic;
using UnityEngine;

public class SlowManager : MonoBehaviour
{
    public static SlowManager Instance;

    List<ISlowable> slowTargets = new List<ISlowable>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void Register(ISlowable target)
    {
        slowTargets.Add(target);
    }

    public void ApplySlow(float factor, SlowTargetType type)
    {
        foreach (ISlowable target in slowTargets)
        {
            if (target.Type == type)
            {
                target.ApplySlow(factor);
            }
        }
    }
}
