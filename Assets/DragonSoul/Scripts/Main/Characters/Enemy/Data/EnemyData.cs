using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/EnemyData")]
public class EnemyData : ScriptableObject
{
    [SerializeField, Header("名前")] string enemyName;
    [SerializeField, Header("最大体力")] float maxHP;
    [SerializeField, Header("ひるみ耐性タイプ")] FlinchType flinchType;

    public float MaxHP => maxHP;
    public FlinchType FlinchType => flinchType;
}
