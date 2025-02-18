using SoundSystem;
using UnityEngine;

public class ResultManager : MonoBehaviour
{
    [SerializeField] GameObject[] buttons;
    [SerializeField] GameObject selectArrow;

    InputReciver Input => InputReciver.Instance;
    enum ResultState { ReturnSelect = 0, ReturnTitle }
    ResultState resultState = ResultState.ReturnSelect;
    bool isPressed = false;

    const float FadeTime = 1.0f;

    void Start()
    {
        SoundManager.Instance.PlayBGMWithFadeIn("Result", FadeTime);        
    }

    void Update()
    {
        if (!isPressed)
        {
            // 選択中のボタンを変更
            if (Input.SelectMoveUp && resultState != ResultState.ReturnSelect)
            {
                resultState--;
                MoveSelectArrow();
                SoundManager.Instance.PlaySe("MenuMove");
            }
            else if (Input.SelectMoveDown && resultState != ResultState.ReturnTitle)
            {
                resultState++;
                MoveSelectArrow();
                SoundManager.Instance.PlaySe("MenuMove");
            }
        }

        if (Input.Decision && !isPressed)
        {
            isPressed = true;
            SoundManager.Instance.PlaySe("Press");
            SoundManager.Instance.StopBGMWithFadeOut(FadeTime);

            switch (resultState)
            {
                case ResultState.ReturnSelect:
                    FadeManager.Instance.LoadScene("SelectScene", FadeTime);
                    break;
                case ResultState.ReturnTitle:
                    FadeManager.Instance.LoadScene("TitleScene", FadeTime);
                    break;
            }
        }
    }

    void MoveSelectArrow()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (i == (int)resultState)
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
