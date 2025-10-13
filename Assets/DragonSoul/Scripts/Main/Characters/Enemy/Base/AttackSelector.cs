using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AttackSelector<TAttackType>
{
    float totalWeight;
    Dictionary<TAttackType, float> weights;

    public AttackSelector(Dictionary<TAttackType,float> initialWeights)
    {
        this.weights = initialWeights;

        InitializeTotalWeight();
    }

    /// <summary>
    /// 攻撃抽選の重さを初期化
    /// </summary>
    void InitializeTotalWeight()
    {
        totalWeight = 0f;
        foreach (var weight in weights)
        {
            totalWeight += weight.Value;
        }
    }

    /// <summary>
    /// 重さの更新
    /// </summary>
    /// <param name="selectedAttack">選ばれた攻撃</param>
    void UpdateTotalWeight(TAttackType selectedAttack)
    {
        var keys = new List<TAttackType>(weights.Keys);
        foreach (var key in keys)
        {
            if (key.Equals(selectedAttack))
            {
                weights[key] = 10;
            }
            else
            {
                weights[key] += 10;
            }
        }

        totalWeight = weights.Values.Sum();
    }

    /// <summary>
    /// 攻撃タイプの抽選
    /// </summary>
    /// <returns>攻撃タイプ</returns>
    public TAttackType ChooseAttack()
    {
        var randomNum = Random.Range(0, totalWeight);

        float currentWeight = 0;

        foreach (var weight in weights)
        {
            currentWeight += weight.Value;

            if (randomNum < currentWeight)
            {
                UpdateTotalWeight(weight.Key);
                return weight.Key;
            }
        }

        return default;
    }
}
