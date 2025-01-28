using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    InputReciver Input => InputReciver.Instance;
    enum TitlePanelState { Start = 0, Option, Quit }
    TitlePanelState selectState = TitlePanelState.Start;
    [SerializeField] TextMeshProUGUI[] texts;

    void Start()
    {
        ChangeTextColor();
    }

    void Update()
    {
        // 選択中のボタンを変更
        if (Input.SelectMoveUp && selectState != TitlePanelState.Start)
        {
            selectState--;
            ChangeTextColor();
        }
        else if (Input.SelectMoveDown && selectState != TitlePanelState.Quit)
        {
            selectState++;
            ChangeTextColor();
        }

        if (Input.Decision)
        {
            switch (selectState)
            {
                case TitlePanelState.Start:
                    SceneManager.LoadScene("SelectScene");
                    break;
                case TitlePanelState.Option:
                    break;
                case TitlePanelState.Quit:
                    QuitGame();
                    break;
            }
        }
    }

    void ChangeTextColor()
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

    void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // ゲームプレイ終了
#else
            Application.Quit(); // ゲームプレイ終了
#endif
    }
}
