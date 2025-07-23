using SoundSystem;
using UnityEngine;

public class CheckUIManagerBase : MonoBehaviour
{
    [SerializeField] GameObject[] buttons;
    [SerializeField] GameObject selectArrow;
    [SerializeField] GameObject previousPanel;
    [SerializeField] NextScene nextScene;   // 次に遷移するシーン

    InputReciver Input => InputReciver.Instance;
    enum CheckPanelState { Yes, No }
    CheckPanelState checkState = CheckPanelState.No;
    string nextSceneName;   // 次に遷移するシーン名
    enum NextScene { Title, Select, Quit }
    bool isPressed = false;

    const float FadeTime = 1.0f;

    void Start()
    {
        gameObject.SetActive(false);
        MoveSelectArrow();

        nextSceneName = nextScene switch
        {
            NextScene.Title => "TitleScene",
            NextScene.Select => "SelectScene",
            _ => ""
        };
    }

    void Update()
    {
        if (!isPressed)
        {
            // 選択中のボタンを変更
            if (Input.SelectMoveLeft && checkState != CheckPanelState.Yes)
            {
                checkState--;
                MoveSelectArrow();
                SoundManager.Instance.PlaySe("MenuMove");
            }
            else if (Input.SelectMoveRight && checkState != CheckPanelState.No)
            {
                checkState++;
                MoveSelectArrow();
                SoundManager.Instance.PlaySe("MenuMove");
            }

            if (Input.Decision)
            {
                isPressed = true;
                SoundManager.Instance.PlaySe("Press");

                switch (checkState)
                {
                    case CheckPanelState.Yes:
                        gameObject.SetActive(false);
                        if (nextScene == NextScene.Quit)
                        {
                            QuitGame();
                        }
                        else
                        {
                            FadeManager.Instance.LoadScene(nextSceneName, FadeTime);
                            SoundManager.Instance.StopBGMWithFadeOut(FadeTime);
                        }
                        break;
                    case CheckPanelState.No:
                        gameObject.SetActive(false);
                        previousPanel.SetActive(true);
                        break;
                }
            }

            if (Input.Return)
            {
                isPressed = true;
                SoundManager.Instance.PlaySe("Press");
                gameObject.SetActive(false);
                previousPanel.SetActive(true);
            }
        }
    }

    void MoveSelectArrow()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (i == (int)checkState)
            {
                selectArrow.transform.position = buttons[i].transform.position;
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

    void OnEnable()
    {
        isPressed = false;
        checkState = CheckPanelState.No;
        MoveSelectArrow();
    }
}
