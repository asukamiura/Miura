using SoundSystem;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [SerializeField] GameObject[] buttons;
    [SerializeField] GameObject selectArrow;
    [SerializeField] GameObject optionPanel;
    [SerializeField] GameObject checkPanel;

    InputReciver Input => InputReciver.Instance;
    enum PausePanelState { ReturnSelect, Option, Close }
    PausePanelState pauseState = PausePanelState.Close;
    bool isPressed = false;

    void Start()
    {
        gameObject.SetActive(false);
        MoveSelectArrow();
    }

    void Update()
    {
        // 選択中のボタンを変更
        if (Input.SelectMoveUp && pauseState != PausePanelState.ReturnSelect)
        {
            pauseState--;
            MoveSelectArrow();
            SoundManager.Instance.PlaySe("MenuMove");
        }
        else if (Input.SelectMoveDown && pauseState != PausePanelState.Close)
        {
            pauseState++;
            MoveSelectArrow();
            SoundManager.Instance.PlaySe("MenuMove");
        }

        if (Input.Decision && !isPressed)
        {
            isPressed = true;
            SoundManager.Instance.PlaySe("Press");

            switch (pauseState)
            {
                case PausePanelState.ReturnSelect:
                    gameObject.SetActive(false);
                    checkPanel.SetActive(true);
                    break;
                case PausePanelState.Option:
                    gameObject.SetActive(false);
                    optionPanel.SetActive(true);
                    break;
                case PausePanelState.Close:
                    gameObject.SetActive(false);
                    GameManager.Instance.ChangeState(GameManager.GameState.Playing);
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

    void OnEnable()
    {
        isPressed = false;
    }
}
