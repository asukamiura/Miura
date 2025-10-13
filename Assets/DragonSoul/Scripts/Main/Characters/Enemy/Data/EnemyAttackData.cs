using UnityEngine;

[CreateAssetMenu(fileName = "EnemyAttackData", menuName = "ScriptableObjects/EnemyAttackData")]
public class EnemyAttackData : ScriptableObject
{
    [SerializeField,Header("攻撃のタイプ")] EnemyAttackType attackType;
    [SerializeField,Header("ダメージ量")] int damage;
     
    public EnemyAttackType AttackType => attackType;
    public int Damage => damage;
}
