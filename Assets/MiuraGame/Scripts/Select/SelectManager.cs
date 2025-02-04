using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectManager : MonoBehaviour
{
    InputReciver Input => InputReciver.Instance;
    enum SelectPanelState { Tutorial = 0, Stage1, Stage2, Stage3, ReturnTitle }
    SelectPanelState selectState = SelectPanelState.Tutorial;
    [SerializeField] TextMeshProUGUI[] texts;

    void Start()
    {
        ChangeTextColor();
    }

    void Update()
    {
        // 選択中のボタンを変更
        if (Input.SelectMoveUp && selectState != SelectPanelState.Tutorial)
        {
            selectState--;
            ChangeTextColor();
        }
        else if (Input.SelectMoveDown && selectState != SelectPanelState.ReturnTitle)
        {
            selectState++;
            ChangeTextColor();
        }

        if (Input.Decision)
        {
            switch (selectState)
            {
                case SelectPanelState.Tutorial:
                    SceneManager.LoadScene("TutorialScene");
                    break;
                case SelectPanelState.Stage1:
                    SceneManager.LoadScene("Stage1Scene");
                    break;
                case SelectPanelState.Stage2:
                    SceneManager.LoadScene("Stage2Scene");
                    break;
                case SelectPanelState.Stage3:
                    SceneManager.LoadScene("Stage3Scene");
                    break;
                case SelectPanelState.ReturnTitle:
                    SceneManager.LoadScene("TitleScene");
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
}
