using SoundSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverPanelManager : MonoBehaviour
{
    [SerializeField] GameObject[] buttons;
    [SerializeField] GameObject selectArrow;
    [SerializeField] GameObject checkPanel;

    InputReciver Input => InputReciver.Instance;
    enum GameOverPanelState { ReturnSelect, Retry }
    GameOverPanelState gameOverState = GameOverPanelState.Retry;
    bool isPressed = false;

    const float FadeTime = 1.0f;

    void Start()
    {
        gameObject.SetActive(false);
        MoveSelectArrow();
    }

    void Update()
    {
        if (!isPressed)
        {
            // 選択中のボタンを変更
            if (Input.SelectMoveLeft && gameOverState != GameOverPanelState.ReturnSelect)
            {
                gameOverState--;
                MoveSelectArrow();
                SoundManager.Instance.PlaySe("MenuMove");
            }
            else if (Input.SelectMoveRight && gameOverState != GameOverPanelState.Retry)
            {
                gameOverState++;
                MoveSelectArrow();
                SoundManager.Instance.PlaySe("MenuMove");
            }

            if (Input.Decision)
            {
                isPressed = true;
                SoundManager.Instance.PlaySe("Press");
                switch (gameOverState)
                {
                    case GameOverPanelState.ReturnSelect:
                        gameObject.SetActive(false);
                        checkPanel.SetActive(true);
                        break;
                    case GameOverPanelState.Retry:
                        FadeManager.Instance.LoadScene(SceneManager.GetActiveScene().name, FadeTime);
                        SoundManager.Instance.StopBGMWithFadeOut(FadeTime);
                        break;
                }
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

    void OnEnable()
    {
        isPressed = false;
    }

    public void ShowGameOverPanel()
    {
        gameObject.SetActive(true);
    }
}
