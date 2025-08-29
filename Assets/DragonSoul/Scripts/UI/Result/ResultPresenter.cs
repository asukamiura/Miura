using SoundSystem;
using System.Collections;
using UnityEngine;

public class ResultPresenter : MonoBehaviour
{
    [SerializeField] ResultView resultView;
    ResultModel model;
    bool isPressed = false;
    bool canInput = false;
    const float FadeTime = 1.0f;
    const int WaitFrame = 150;

    void Awake()
    {
        model = new ResultModel();
    }

    void Start()
    {
        StartCoroutine(WaitCanInput(WaitFrame));
    }

    void OnEnable()
    {
        resultView.OnPressedUp += TriggerPressedUp;
        resultView.OnPressedDown += TriggerPressedDown;
        resultView.OnPressedDecision += TriggerPressedDecision;

        resultView.SetScore(ScoreManager.Instance.CurrentScore);
        resultView.SetClearTime(ScoreManager.Instance.ClearTime);
        resultView.SetJustDodgeCount(ScoreManager.Instance.JustDodgeCount);
        resultView.SetJustGuardCount(ScoreManager.Instance.JustGuardCount);
        resultView.SetRank(ScoreManager.Instance.GetRank());
    }

    void OnDisable()
    {
        resultView.OnPressedUp -= TriggerPressedUp;
        resultView.OnPressedDown -= TriggerPressedDown;
        resultView.OnPressedDecision -= TriggerPressedDecision;

        ScreenShotManager.Instance.DeleteScreenShot();
    }

    void TriggerPressedUp()
    {
        if (!canInput || isPressed || model.CurrentState == ResultModel.ResultState.Select) { return; }

        SoundManager.Instance.PlaySe("MenuMove");
        model.ChangeStateUp();
        resultView.MoveSelectArrow((int)model.CurrentState);
    }

    void TriggerPressedDown()
    {
        if (!canInput || isPressed || model.CurrentState == ResultModel.ResultState.Title) { return; }

        SoundManager.Instance.PlaySe("MenuMove");
        model.ChangeStateDown();
        resultView.MoveSelectArrow((int)model.CurrentState);
    }

    void TriggerPressedDecision()
    {
        if (!canInput || isPressed) { return; }

        isPressed = true;

        SoundManager.Instance.PlaySe("Press");

        switch (model.CurrentState)
        {
            case ResultModel.ResultState.Select:
                FadeManager.Instance.LoadScene("SelectScene", FadeTime);
                break;

            case ResultModel.ResultState.Title:
                FadeManager.Instance.LoadScene("TitleScene", FadeTime);
                break;
        }
    }

    /// <summary>
    /// リザルト演出が終わったら入力を許可
    /// </summary>
    /// <param name="waitFrame">待機時間</param>
    /// <returns></returns>
    IEnumerator WaitCanInput(int waitFrame)
    {
        for (int i = 0; i < waitFrame; i++)
        {
            yield return null;
        }

        canInput = true;
    }
}
