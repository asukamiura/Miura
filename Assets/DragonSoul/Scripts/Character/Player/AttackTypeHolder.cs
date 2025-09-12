using Player;
using UnityEngine;

public class AttackTypeHolder : MonoBehaviour
{
    PlayerAttackType currentAttackType = PlayerAttackType.None;

    /// <summary>
    /// 攻撃タイプを設定
    /// </summary>
    /// <param name="attackType">攻撃タイプ</param>
    public void SetAttackType(PlayerAttackType attackType)
    {
        currentAttackType = attackType;
    }

    /// <summary>
    /// 攻撃タイプを取得
    /// </summary>
    /// <returns>現在の攻撃タイプ</returns>
    public PlayerAttackType GetAttackType()
    {
        return currentAttackType;
    }
}
