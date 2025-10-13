using UnityEngine;

[CreateAssetMenu(fileName = "PowerUpData", menuName = "ScriptableObjects/PowerUpData")]
public class PowerUpData : ScriptableObject
{
    [SerializeField, Header("アクション名")] string actionName;
    [SerializeField, Header("必要ジャストポイント")] int cost;
    [SerializeField, Header("攻撃力アップ倍率")] float multiplier;    
    [SerializeField, Header("効果時間")] float duration;

    public int Cost => cost;
    public float Multiplier => multiplier;
    public float Duration => duration;
}
