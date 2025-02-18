using SoundSystem;
using UnityEngine;

public class SelectManager : MonoBehaviour
{
    [SerializeField] GameObject[] buttons;
    [SerializeField] GameObject selectArrow;
    [SerializeField] GameObject[] enemyPanels;
    [SerializeField] GameObject checkPanel;

    InputReciver Input => InputReciver.Instance;
    enum SelectPanelState { Tutorial = 0, Stage1, Stage2, Stage3, ReturnTitle }
    SelectPanelState selectState = SelectPanelState.Tutorial;
    bool isPressed = false;

    const float FadeTime = 1.0f;

    public static int InStageNum = 0;

    void Start()
    {
        SoundManager.Instance.PlayBGMWithFadeIn("Select");

        if (InStageNum == 1 || InStageNum == 2 || InStageNum == 3)
        {
            switch (InStageNum)
            {
                case 0: selectState = SelectPanelState.Tutorial; break;
                case 1: selectState = SelectPanelState.Stage1; break;
                case 2: selectState = SelectPanelState.Stage2; break;
                case 3: selectState = SelectPanelState.Stage3; break;
            }
        }

        MoveSelectArrow();
        ChangeEnemyPanel();
    }

    void Update()
    {
        if (!isPressed)
        {
            // 選択中のボタンを変更
            if (Input.SelectMoveUp && selectState != SelectPanelState.Tutorial)
            {
                selectState--;
                MoveSelectArrow();
                ChangeEnemyPanel();
                SoundManager.Instance.PlaySe("MenuMove");
            }
            else if (Input.SelectMoveDown && selectState != SelectPanelState.ReturnTitle)
            {
                selectState++;
                MoveSelectArrow();
                ChangeEnemyPanel();
                SoundManager.Instance.PlaySe("MenuMove");
            }

            if (Input.Decision)
            {
                isPressed = true;
                SoundManager.Instance.PlaySe("Press");
                SoundManager.Instance.StopBGMWithFadeOut(FadeTime);

                switch (selectState)
                {
                    case SelectPanelState.Tutorial:
                        FadeManager.Instance.LoadScene("TutorialScene", FadeTime);
                        InStageNum = 0;
                        break;
                    case SelectPanelState.Stage1:
                        FadeManager.Instance.LoadScene("Stage1Scene", FadeTime);
                        InStageNum = 1;
                        break;
                    case SelectPanelState.Stage2:
                        FadeManager.Instance.LoadScene("Stage2Scene", FadeTime);
                        InStageNum = 2;
                        break;
                    case SelectPanelState.Stage3:
                        FadeManager.Instance.LoadScene("Stage3Scene", FadeTime);
                        InStageNum = 3;
                        break;
                    case SelectPanelState.ReturnTitle:
                        selectArrow.SetActive(false);
                        gameObject.SetActive(false);
                        checkPanel.SetActive(true);
                        InStageNum = 0;
                        break;
                }
            }

            if (Input.Return)
            {
                isPressed = true;
                selectArrow.SetActive(false);
                gameObject.SetActive(false);
                checkPanel.SetActive(true);
                InStageNum = 0;
            }
        }

    }

    void MoveSelectArrow()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (i == (int)selectState)
            {
                selectArrow.transform.position = buttons[i].transform.position;
            }
        }
    }

    void ChangeEnemyPanel()
    {
        for (int i = 0; i < enemyPanels.Length; i++)
        {
            if (i == (int)selectState)
            {
                enemyPanels[i].SetActive(true);
            }
            else
            {
                enemyPanels[i].SetActive(false);
            }
        }
    }

    void OnEnable()
    {
        isPressed = false;  
        selectArrow.SetActive(true);
    }
}
