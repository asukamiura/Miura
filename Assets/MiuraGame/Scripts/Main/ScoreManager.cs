using Player;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    readonly Dictionary<string, int> scoreDic = new Dictionary<string, int>
    {
        { "AttackNormal1", 10 },
        { "AttackNormal2", 20 },
        { "AttackNormal3", 30 },
        { "AttackSpecial", 300 },
        { "AttackCharge" , 200 },
        { "AttackUltimate", 500 },
        { "Fast", 300 },
        { "Just", 500 },
        { "Late", 300 },       
        { "TimeA", 2000 },
        { "TimeB", 1000 },
        { "TimeC", 500 },
        { "JustBonus", 100 },
        { "NoDamageBonus", 1000 },
        { "Damage", -50 },
    };

    int totalScore = 0;     // 合計スコア
    float currentTime = 0;  // 経過時間
    enum Rank
    {
        D,
        C,
        B,
        A,
        S,
    }

    Rank rank = Rank.D;

    public static ScoreManager Instance { get; set; }

    public static int JustGuardCount { get; set; } = 0;
    public static int JustDodgeCount { get; set; } = 0;
    public static string CurrentRank { get; set; } = "D";
    public static int CurrentScore { get; set; } = 0;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        PlayerDash.OnJudgeDodgeTiming += AddScore;
        PlayerDash.OnJudgeDodgeTiming += CountUpJustDodge;
        PlayerGuard.OnJudgeGuardTiming += AddScore;
        PlayerGuard.OnJudgeGuardTiming += CountUpJustGuard;
    }

    void Update()
    {
        currentTime += Time.deltaTime;

        if (totalScore >= 8000)
        {
            rank = Rank.S;
        }
        else if (totalScore >= 4000 && totalScore <= 7999)
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

    void CountUpJustDodge(string timing)
    {
        if (timing == "Just")
        {
            JustDodgeCount++;
        }
    }

    void CountUpJustGuard(string timing)
    {
        if (timing == "Just")
        {
            JustGuardCount++;
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

    /// <summary>
    /// ランク取得用メソッド
    /// </summary>
    /// <returns>ランク</returns>
    public string GetRank()
    {
        string rankName = "";
        switch (rank)
        {
            case Rank.S: rankName = "S"; break;
            case Rank.A: rankName = "A"; break;
            case Rank.B: rankName = "B"; break;
            case Rank.C: rankName = "C"; break;
            case Rank.D: rankName = "D"; break;
        }
        return rankName;
    }

    private void OnDisable()
    {
        CurrentRank = GetRank();
        CurrentScore = totalScore;
        Debug.Log(CurrentRank);
        PlayerDash.OnJudgeDodgeTiming -= AddScore;
        PlayerGuard.OnJudgeGuardTiming -= AddScore;
        PlayerDash.OnJudgeDodgeTiming -= CountUpJustDodge;
        PlayerGuard.OnJudgeGuardTiming -= CountUpJustGuard;
    }
}
