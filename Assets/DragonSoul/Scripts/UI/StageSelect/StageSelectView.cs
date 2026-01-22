using System;
using TMPro;
using UnityEngine;

public class StageSelectView : MonoBehaviour
{
    InputReceiver Input => InputReceiver.Instance;

    [SerializeField] GameObject[] buttons;
    [SerializeField] GameObject selectArrow;
    [SerializeField] TextMeshProUGUI[] scoreTexts;
    [SerializeField] TextMeshProUGUI[] rankTexts;

    public Action OnPressedRight;
    public Action OnPressedLeft;
    public Action OnPressedDecision;
    public Action OnPressedReturn;

    void Update()
    {
        if (Input.SelectMoveLeft)
        {
            OnPressedLeft?.Invoke();
        }

        if (Input.SelectMoveRight)
        {
            OnPressedRight?.Invoke();
        }

        if (Input.Decision)
        {
            OnPressedDecision?.Invoke();
        }

        if (Input.Return)
        {
            OnPressedReturn?.Invoke();
        }
    }

    public void MoveSelectArrow(int index)
    {
        selectArrow.transform.position = new Vector3(
            buttons[index].transform.position.x,
            selectArrow.transform.position.y,
            buttons[index].transform.position.z
        );
    }

    public void SetHighScore(int stageNum, int score)
    {
        if (score == 0)
        {
            scoreTexts[stageNum].text = "-------";
        }
        else
        {
            scoreTexts[stageNum].text = score.ToString();
        }
    }

    public void SetBestRank(int stageNum, int rank)
    {
        string bestRank = rank switch
        {
            0 => "S",
            1 => "A",
            2 => "B",
            3 => "C",
            4 => "-",
            _ => ""
        };

        rankTexts[stageNum].text = bestRank;
    }
}
