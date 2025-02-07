using SoundSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckManager : MonoBehaviour
{
    [SerializeField] GameObject[] buttons;
    [SerializeField] GameObject selectArrow;
    [SerializeField] GameObject previousPanel;
    [SerializeField] NextScene nextScene;   // 次に遷移するシーン

    InputReciver Input => InputReciver.Instance;
    enum GameOverPanelState { Yes, No }
    GameOverPanelState gameOverState = GameOverPanelState.No;
    string nextSceneName;   // 次に遷移するシーン名
    enum NextScene { Main, Select, Quit }
    bool isPressed = false;

    void Start()
    {
        gameObject.SetActive(false);
        MoveSelectArrow();

        nextSceneName = nextScene switch
        {
            NextScene.Main => "MainScene",
            NextScene.Select => "SelectScene",
            _ => ""
        };
    }

    void Update()
    {
        if (!isPressed)
        {
            // 選択中のボタンを変更
            if (Input.SelectMoveLeft && gameOverState != GameOverPanelState.Yes)
            {
                gameOverState--;
                MoveSelectArrow();
                SoundManager.Instance.PlaySe("MenuMove");
            }
            else if (Input.SelectMoveRight && gameOverState != GameOverPanelState.No)
            {
                gameOverState++;
                MoveSelectArrow();
                SoundManager.Instance.PlaySe("MenuMove");
            }
        }

        if (Input.Decision && !isPressed)
        {
            isPressed = true;
            SoundManager.Instance.PlaySe("Press");

            switch (gameOverState)
            {
                case GameOverPanelState.Yes:
                    gameObject.SetActive(false);
                    if (nextScene == NextScene.Quit)
                    {
                        QuitGame();
                    }
                    else
                    {
                        Time.timeScale = 1;
                        FadeManager.Instance.LoadScene(nextSceneName);
                        SoundManager.Instance.StopBGMWithFadeOut();
                    }
                    break;
                case GameOverPanelState.No:
                    gameObject.SetActive(false);
                    previousPanel.SetActive(true);
                    break;
            }
        }
    }

    void MoveSelectArrow()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (i == (int)gameOverState)
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
    }
}
