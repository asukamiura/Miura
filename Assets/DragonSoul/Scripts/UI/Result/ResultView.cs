using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultView : MonoBehaviour
{
    InputReciver Input => InputReciver.Instance;

    [SerializeField] GameObject[] buttons;
    [SerializeField] GameObject selectArrow;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI clearTimeText;
    [SerializeField] TextMeshProUGUI justDodgeCount;
    [SerializeField] TextMeshProUGUI justGuardCount;
    [SerializeField] Image rankImage;
    [SerializeField] Sprite[] rankSprites;

    public Action OnPressedUp;
    public Action OnPressedDown;
    public Action OnPressedDecision;

    void Update()
    {
        if (Input.SelectMoveUp)
        {
            OnPressedUp?.Invoke();
        }

        if (Input.SelectMoveDown)
        {
            OnPressedDown?.Invoke();
        }

        if (Input.Decision)
        {
            OnPressedDecision?.Invoke();
        }
    }

    public void MoveSelectArrow(int index)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (i == index)
            {
                selectArrow.transform.position = buttons[i].transform.position;
            }
        }
    }

    /// <summary>
    /// 獲得スコアをテキストに設定
    /// </summary>
    /// <param name="score">スコア</param>
    public void SetScore(int score)
    {
        scoreText.text = score.ToString();
    }

    /// <summary>
    /// クリアタイムをテキストに設定
    /// </summary>
    /// <param name="time">クリアタイム</param>
    public void SetClearTime(float time)
    {
        float minutes = time / 60;

        float second = time % 60;

        if (minutes < 10 & second < 10)
        {
            clearTimeText.text = "0" + (int)minutes + ":0" + (int)second;
        }
        else if (minutes < 10)
        {
            clearTimeText.text = "0" + (int)minutes + ":" + (int)second;
        }
        else if (second < 10)
        {
            clearTimeText.text = (int)minutes + ":0" + (int)second;
        }
        else
        {
            clearTimeText.text = (int)minutes + ":" + (int)second;
        }
    }

    /// <summary>
    /// ジャスト回避に成功した回数をテキストに設定
    /// </summary>
    /// <param name="count">成功回数</param>
    public void SetJustDodgeCount(int count)
    {
        justDodgeCount.text = count.ToString();
    }

    /// <summary>
    /// ジャストガードに成功した回数をテキストに設定
    /// </summary>
    /// <param name="count">成功回数</param>
    public void SetJustGuardCount(int count)
    {
        justGuardCount.text = count.ToString();
    }

    /// <summary>
    /// 獲得ランクのUI画像を設定
    /// </summary>
    /// <param name="rank">獲得ランク</param>
    public void SetRank(string rank)
    {
        switch (rank)
        {
            case "S":
                rankImage.sprite = rankSprites[0];
                break;

            case "A":
                rankImage.sprite = rankSprites[1];
                break;

            case "B":
                rankImage.sprite = rankSprites[2];
                break;

            case "C":
                rankImage.sprite = rankSprites[3];
                break;

            case "D":
                rankImage.sprite = rankSprites[4];
                break;
        }
    }
}
