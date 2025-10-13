using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackSequenceData", menuName = "ScriptableObjects/AttackSequenceData")]
public class AttackSequenceData : ScriptableObject
{
    [SerializeField, Header("段階攻撃リスト")] List<PlayerAttackData> playerAttackDatas = new List<PlayerAttackData>();

    public PlayerAttackData GetAttackData(int step)
    {
        return playerAttackDatas[step];
    }

    public int CombStepCount => playerAttackDatas.Count;
}
