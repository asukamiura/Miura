using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    InputReciver Input => InputReciver.Instance;
    private enum TitlePanelState { Start = 0, Option, Quit }
    private TitlePanelState selectState = TitlePanelState.Start;
    [SerializeField] private TextMeshProUGUI[] texts;

    private void Start()
    {
        ChangeTextColor();
    }

    private void Update()
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
