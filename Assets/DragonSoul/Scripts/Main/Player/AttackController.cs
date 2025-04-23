using Player;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    AttackType currentAttackType = AttackType.None;

    /// <summary>
    /// 攻撃タイプを設定
    /// </summary>
    /// <param name="attackType">攻撃タイプ</param>
    public void SetAttackType(AttackType attackType)
    {
        currentAttackType = attackType;
    }

    /// <summary>
    /// 攻撃タイプを取得
    /// </summary>
    /// <returns>現在の攻撃タイプ</returns>
    public AttackType GetAttackType()
    {
        return currentAttackType;
    }
}
