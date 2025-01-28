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

    private void Start()
    {
        ChangeTextColor();
    }

    private void Update()
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

        if (Input.Decision)
        {
            switch (selectState)
            {
                case TitlePanelState.ReturnSelect:
                    SceneManager.LoadScene("SelectScene");
                    break;
                case TitlePanelState.ReturnTitle:
                    SceneManager.LoadScene("TitleScene");
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
