using SoundSystem;
using UnityEngine;

public class TitleManager : MonoBehaviour
{
    [SerializeField] GameObject savePrefab;
    [SerializeField] GameObject[] buttons;
    [SerializeField] GameObject selectArrow;
    [SerializeField] GameObject optionPanel;
    [SerializeField] GameObject checkPanel;

    InputReciver Input => InputReciver.Instance;
    GameObject saveObj;
    SaveManager saveManager;
    enum PausePanelState { Start, Option, Quit }
    PausePanelState pauseState = PausePanelState.Start;
    bool isPressed = false;

    void Awake()
    {
        saveObj = Instantiate(savePrefab);
        saveObj.name = "SaveManager";       // そのままだと(Clone)がつくので名前上書き
        saveManager = saveObj.GetComponent<SaveManager>();
        if (!saveManager.SaveDataCheck())
        {
            saveManager.CreateSaveData();
        }
    }

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        SoundManager.Instance.PlayBGMWithFadeIn("Title");

        MoveSelectArrow();
    }

    void Update()
    {
        if (!isPressed)
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
                isPressed = true;
                SoundManager.Instance.PlaySe("Press");

                switch (pauseState)
                {
                    case PausePanelState.Start:
                        SoundManager.Instance.StopBGMWithFadeOut();
                        FadeManager.Instance.LoadScene("SelectScene");
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
