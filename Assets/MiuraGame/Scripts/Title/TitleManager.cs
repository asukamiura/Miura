using SoundSystem;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    [SerializeField] GameObject[] buttons;
    [SerializeField] GameObject selectArrow;
    [SerializeField] GameObject optionPanel;
    [SerializeField] GameObject checkPanel;

    InputReciver Input => InputReciver.Instance;
    enum PausePanelState { Start, Option, Quit }
    PausePanelState pauseState = PausePanelState.Start;

    void Start()
    {
        MoveSelectArrow();
    }

    void Update()
    {
        // 選択中のボタンを変更
        if (Input.SelectMoveUp && pauseState != PausePanelState.Start)
        {
            pauseState--;
            MoveSelectArrow();
            SoundManager.Instance.PlaySe("MenuMove");
        }
        else if (Input.SelectMoveDown && pauseState != PausePanelState.Quit)
        {
            pauseState++;
            MoveSelectArrow();
            SoundManager.Instance.PlaySe("MenuMove");
        }

        if (Input.Decision)
        {
            SoundManager.Instance.PlaySe("Press");

            switch (pauseState)
            {
                case PausePanelState.Start:
                    SceneManager.LoadScene("SelectScene");
                    break;
                case PausePanelState.Option:
                    gameObject.SetActive(false);
                    optionPanel.SetActive(true);
                    break;
                case PausePanelState.Quit:
                    gameObject.SetActive(false);
                    checkPanel.SetActive(true);
                    break;
            }
        }
    }

    void MoveSelectArrow()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (i == (int)pauseState)
            {
                selectArrow.transform.position = buttons[i].transform.position;
            }
        }
    }
}
