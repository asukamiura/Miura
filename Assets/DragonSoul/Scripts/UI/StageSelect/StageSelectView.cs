using System;
using TMPro;
using UnityEngine;

public class StageSelectView : MonoBehaviour
{
    InputReciver Input => InputReciver.Instance;

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
        Vector3 selectArrowPos = selectArrow.transform.position;

        for (int i = 0; i < buttons.Length; i++)
        {
            if (i == index)
            {
                selectArrowPos.x = buttons[i].transform.position.x;
                selectArrowPos.y = selectArrow.transform.position.y;
                selectArrowPos.z = buttons[i].transform.position.z;
            }

            selectArrow.transform.position = selectArrowPos;
        }
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
