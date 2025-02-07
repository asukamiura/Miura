using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultManager : MonoBehaviour
{
    InputReciver Input => InputReciver.Instance;
    private enum TitlePanelState { ReturnSelect = 0, ReturnTitle}
    private TitlePanelState selectState = TitlePanelState.ReturnSelect;
    [SerializeField] private TextMeshProUGUI[] texts;
    bool isPressed = false;

    private void Start()
    {
        ChangeTextColor();
    }

    private void Update()
    {
        if (!isPressed)
        {
            // 選択中のボタンを変更
            if (Input.SelectMoveUp && selectState != TitlePanelState.ReturnSelect)
            {
                selectState--;
                ChangeTextColor();
            }
            else if (Input.SelectMoveDown && selectState != TitlePanelState.ReturnTitle)
            {
                selectState++;
                ChangeTextColor();
            }
        }

        if (Input.Decision && !isPressed)
        {
            isPressed = true;
            switch (selectState)
            {
                case TitlePanelState.ReturnSelect:
                    FadeManager.Instance.LoadScene("SelectScene");
                    break;
                case TitlePanelState.ReturnTitle:
                    FadeManager.Instance.LoadScene("TitleScene");
                    break;              
            }
        }
    }

    private void ChangeTextColor()
    {
        // テキストの色を変更
        for (int i = 0; i < texts.Length; i++)
        {
            if (i == (int)selectState)
            {
                texts[i].color = Color.red;
            }
            else
            {
                texts[i].color = Color.black;
            }
        }
    }
}
