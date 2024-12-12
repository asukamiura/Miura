using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    InputReciver Input => InputReciver.Instance;
    private enum TitlSelectlState { Start = 0, Option, Quit }
    private TitlSelectlState selectState = TitlSelectlState.Start;
    [SerializeField] private TextMeshProUGUI[] texts;

    private void Start()
    {
        ChangeTextColor();
    }

    private void Update()
    {
        // 選択中のボタンを変更
        if (Input.SelectMoveUp && selectState != TitlSelectlState.Start)
        {
            selectState--;
            ChangeTextColor();
        }
        else if (Input.SelectMoveDown && selectState != TitlSelectlState.Quit)
        {
            selectState++;
            ChangeTextColor();
        }

        if (Input.Decision)
        {
            switch (selectState)
            {
                case TitlSelectlState.Start:
                    SceneManager.LoadScene("MainScene");
                    break;
                case TitlSelectlState.Option:
                    break;
                case TitlSelectlState.Quit:
                    QuitGame();
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

    private void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // ゲームプレイ終了
#else
            Application.Quit(); // ゲームプレイ終了
#endif
    }
}
