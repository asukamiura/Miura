using System;
using UnityEngine;

public class JustPointManager : MonoBehaviour
{
    [SerializeField] int justPoint = 5;

    const int MinJustPoint = 0;    // ジャストポイント下限

    public int MaxJustPoint { get; set; }   // ジャストポイント上限
    public int JustPoint => justPoint;

    public Action<int> OnJustPointChanged;
    
    void Awake()
    {
        MaxJustPoint = justPoint;
    }

    /// <summary>
    /// ジャストポイント増加処理
    /// </summary>
    /// <param name="point">ジャストポイント増加量</param>
    public void AddJustPoint(int point)
    {
        justPoint = Mathf.Clamp(justPoint + point, MinJustPoint, MaxJustPoint);

        OnJustPointChanged?.Invoke(justPoint);
    }

    /// <summary>
    /// ジャストポイント減少処理
    /// </summary>
    /// <param name="point">ジャストポイント減少量</param>
    public void UseJustPoint(int point)
    {
        justPoint = Mathf.Clamp(justPoint - point, MinJustPoint, MaxJustPoint);

        OnJustPointChanged?.Invoke(justPoint);
    }
}
