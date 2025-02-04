using SoundSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] GameObject[] buttons;
    [SerializeField] GameObject selectArrow;
    [SerializeField] GameObject checkPanel;

    InputReciver Input => InputReciver.Instance;
    enum GameOverPanelState { ReturnSelect, Retry }
    GameOverPanelState gameOverState = GameOverPanelState.Retry;
    float selectArrowPositionY = 0f;

    void Start()
    {
        gameObject.SetActive(false);
        selectArrowPositionY = selectArrow.transform.position.y;
        MoveSelectArrow();
    }

    void Update()
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
            SoundManager.Instance.PlaySe("Press");
            switch (gameOverState)
            {
                case GameOverPanelState.ReturnSelect:
                    gameObject.SetActive(false);
                    checkPanel.SetActive(true);
                    break;
                case GameOverPanelState.Retry:
                    Time.timeScale = 1;
                    //SceneManager.LoadScene("");
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
                selectArrow.transform.position = new Vector2(buttons[i].transform.position.x, selectArrowPositionY);
            }
        }
    }
}
