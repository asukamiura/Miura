using System;
using UnityEngine;

public interface IAttackProcessor
{
    public event Action<HitInfo> OnAttackHit;
}

public struct HitInfo
{
    public Vector3 hitPoint;
    public PlayerAttackData attackData;
    public float damage;
    public bool inPowerUp;
}
