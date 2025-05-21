using SoundSystem;
using System;
using UnityEngine;

public class PausePanelManager : MonoBehaviour
{
    [SerializeField] GameObject[] buttons;
    [SerializeField] GameObject selectArrow;
    [SerializeField] GameObject optionPanel;
    [SerializeField] GameObject checkPanel;

    InputReciver Input => InputReciver.Instance;
    enum PausePanelState { ReturnSelect, Option, Close }
    PausePanelState currentState = PausePanelState.Close;
    bool isPressed = false;

    public Action ReturnSelectPressed;
    public Action SoundPressed;
    public Action ClosePressed;

    void Start()
    {
        HidePausePanel();
        MoveSelectArrow();
    }

    void Update()
    {
        if (!isPressed)
        {
            // 選択中のボタンを変更
            if (Input.SelectMoveUp && currentState != PausePanelState.ReturnSelect)
            {
                currentState--;
                MoveSelectArrow();
                SoundManager.Instance.PlaySe("MenuMove");
            }
            else if (Input.SelectMoveDown && currentState != PausePanelState.Close)
            {
                currentState++;
                MoveSelectArrow();
                SoundManager.Instance.PlaySe("MenuMove");
            }

            if (Input.Decision)
            {
                isPressed = true;
                SoundManager.Instance.PlaySe("Press");

                switch (currentState)
                {
                    case PausePanelState.ReturnSelect:
                        HidePausePanel();
                        checkPanel.SetActive(true);
                        break;
                    case PausePanelState.Option:
                        HidePausePanel();
                        optionPanel.SetActive(true);
                        break;
                    case PausePanelState.Close:
                        //HidePausePanel();
                        //GameManager.Instance.ChangeState(GameManager.GameState.Playing);                       
                        ClosePressed?.Invoke();
                        break;
                }
            }

            if (Input.Return)
            {
                isPressed = true;
                SoundManager.Instance.PlaySe("Press");
                //currentState = PausePanelState.Close;
                //GameManager.Instance.ChangeState(GameManager.GameState.Playing);
                
                ClosePressed?.Invoke();
            }
        }       
    }

    void MoveSelectArrow()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (i == (int)currentState)
            {
                selectArrow.transform.position = buttons[i].transform.position;
            }
        }
    }

    void OnEnable()
    {
        isPressed = false;
        MoveSelectArrow();
    }

    public void ShowPausePanel()
    {
        gameObject.SetActive(true);
    }

    public void HidePausePanel()
    {
        gameObject.SetActive(false);
    }
}
