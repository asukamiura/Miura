using Player;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    readonly Dictionary<string, int> scoreDic = new Dictionary<string, int>
    {
        { (AttackType.Normal1).ToString(), 10 },
        { (AttackType.Normal2).ToString(), 20 },
        { (AttackType.Normal3).ToString(), 30 },
        { (AttackType.Special1_1).ToString(), 300 },
        { (AttackType.Special1_2).ToString(), 300 },
        { (AttackType.Special1_3).ToString(), 300 },
        { (AttackType.Special2_1).ToString(), 300 },
        { (AttackType.Special2_2).ToString(), 300 },
        { (AttackType.Special2_3).ToString(), 300 },
        { (AttackType.Special2_4).ToString(), 300 },
        { (AttackType.Ultimate).ToString(), 500 },
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

    int justDodgeCount = 0; // ジャスト回避カウント
    int justGuardCount = 0; // ジャストガードカウント
    int totalScore = 0;     // 合計スコア
    float currentTime = 0;  // 経過時間
    bool isStopTimer = false;

    enum Rank 
    {
        S = 0,
        A,
        B,
        C,
        D,
    }

    Rank rank = Rank.D;

    public static ScoreManager Instance { get; set; }

    public int JustDodgeCount { get { return justDodgeCount; } }
    public int JustGuardCount { get { return justGuardCount; } }
    public string CurrentRank { get; set; } = "D";
    public int CurrentScore { get { return totalScore; } }
    public float ClearTime { get { return currentTime; } }
    
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
        if (!isStopTimer)
        {
            currentTime += Time.deltaTime;
        }
    }
  
    void CountUpJustDodge(string timing)
    {
        if (timing == "Just")
        {
            justDodgeCount++;
        }
    }

    void CountUpJustGuard(string timing)
    {
        if (timing == "Just")
        {
            justGuardCount++;
        }
    }

    public void StopTimer()
    {
        isStopTimer = true;
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

        // ランクを更新
        UpdateRank();
    }

    /// <summary>
    /// スコア減算処理
    /// </summary>
    /// <param name="value">減算量</param>
    public void SubtractScore(int value)
    {
        totalScore -= value;

        // ランクを更新
        UpdateRank();
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

    /// <summary>
    /// 現在のランクを更新
    /// </summary>
    void UpdateRank()
    {
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

    public void UpdateHighScore()
    {
        SaveManager.Instance.SaveHighScore((int)StageSelectModel.inStageNum - 1, totalScore);
    }

    public void UpdateBestRank()
    {
        SaveManager.Instance.SaveBestRank((int)StageSelectModel.inStageNum - 1, (int)rank);
    }

    void OnDisable()
    {
        CurrentRank = GetRank();

        PlayerDash.OnJudgeDodgeTiming -= AddScore;
        PlayerGuard.OnJudgeGuardTiming -= AddScore;
        PlayerDash.OnJudgeDodgeTiming -= CountUpJustDodge;
        PlayerGuard.OnJudgeGuardTiming -= CountUpJustGuard;
    }
}
