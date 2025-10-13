using UnityEngine;

[CreateAssetMenu(fileName = "PlayerParameterData", menuName = "ScriptableObjects/PlayerParameterData")]
public class PlayerParameterData : ScriptableObject
{
    [SerializeField, Header("最大体力")] float maxHP;
    [SerializeField, Header("攻撃力")] float attackPower;
    [SerializeField, Header("移動速度")] float moveSpeed;    
    [SerializeField, Header("必要必殺技ゲージ量")] int ultCost;    
    [SerializeField, Header("ジャストポイント")] int justPoint;
    [SerializeField, Header("ダッシュのクールタイム")] float coolTime;

    public float MaxHP => maxHP;
    public float AttackPower => attackPower;    
    public float MoveSpeed => moveSpeed;
    public int UltCost => ultCost;
    public int JustPoint => justPoint;
    public float CoolTime => coolTime;
}
