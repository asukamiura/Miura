using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private readonly Dictionary<string, int> scoreDic = new Dictionary<string, int>
    {
        { "AttackNormal1", 10 },
        { "AttackNormal2", 20 },
        { "AttackNormal3", 30 },
        { "AttackSpecial", 300 },
        { "AttackCharge" , 200 },
        { "AttackUltimate", 500 },
        { "FastDodge", 300 },
        { "JustDodge", 500 },
        { "LateDodge", 300 },
        { "FastGaurd", 300 },
        { "JustGaurd", 500 },
        { "LateGaurd", 300 },
        { "TimeA", 2000 },
        { "TimeB", 1000 },
        { "TimeC", 500 },
        { "JustBonus", 100 },
        { "NoDamageBonus", 1000 },
        { "Damage", -50 },
    };

    private int totalScore = 0;     // 合計スコア
    private float currentTime = 0;  // 経過時間
    private enum Rank
    {
        D,
        C,
        B,
        A,
        S,
    }

    private Rank rank = Rank.D;

    private void Update()
    {
        currentTime += Time.deltaTime; 
        
        if (totalScore >= 5000)
        {
            rank = Rank.S;
        }
        else if (totalScore >= 4000 && totalScore <= 4999)
        {
            rank = Rank.A;
        }
        else if (totalScore >= 3000 && totalScore <= 3999)
        {
            rank = Rank.B;
        }
        else if (totalScore >= 2000 && totalScore <= 2999)
        {
            rank = Rank.C;
        }
        else
        {
            rank = Rank.D;
        }
    }

    /// <summary>
    /// スコア加算処理
    /// </summary>
    /// <param name="name">加算するスコアの登録名</param>
    public void AddScore(string name)
    {
        if (scoreDic.TryGetValue(name, out int score))
        {
            totalScore += score;
        }
    }

    /// <summary>
    /// スコア減算処理
    /// </summary>
    /// <param name="value">減算量</param>
    public void SubtractScore(int value)
    {
        totalScore -= value;
    }
}
