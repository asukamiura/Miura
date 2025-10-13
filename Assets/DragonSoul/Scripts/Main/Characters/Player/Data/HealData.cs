using UnityEngine;

[CreateAssetMenu(fileName = "HealData", menuName = "ScriptableObjects/HealData")]
public class HealData : ScriptableObject
{
    [SerializeField, Header("アクション名")] string actionName;
    [SerializeField, Header("必要ジャスポイント")] int cost;
    [SerializeField, Header("回復量")] int healVal;

    public int Cost => cost;
    public int HealVal => healVal;
}
