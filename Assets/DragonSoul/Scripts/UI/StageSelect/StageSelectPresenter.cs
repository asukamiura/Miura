using SoundSystem;
using UnityEngine;

public class StageSelectPresenter : MonoBehaviour
{
    [SerializeField] ConfirmDialogView confirmDialogView;
    [SerializeField] ConfirmDialogPresenter confirmDialogPresenter;
    [SerializeField] StageSelectView view;
    StageSelectModel model;
    bool isDialogOpen = false;
    bool isPressed = false;

    void Awake()
    {
        model = new StageSelectModel();
    }

    void Start()
    {
        view.OnPressedLeft += TriggerPressedLeft;
        view.OnPressedRight += TriggerPressedRight;
        view.OnPressedDecision += TriggerPressedDecision;
        view.OnPressedReturn += TriggerPressedReturn;

        view.MoveSelectArrow((int)model.CurrentState);
        ShowHighScores();
        ShowBestRanks();
    }

    void TriggerPressedLeft()
    {
        if (isDialogOpen || isPressed) { return; }

        SoundManager.Instance.PlaySe("MenuMove");
        model.ChangeStateLeft();
        view.MoveSelectArrow((int)model.CurrentState);
    }

    void TriggerPressedRight()
    {
        if (isDialogOpen || isPressed) { return; }

        SoundManager.Instance.PlaySe("MenuMove");
        model.ChangeStateRight();
        view.MoveSelectArrow((int)model.CurrentState);
    }

    void TriggerPressedDecision()
    {
        if (isDialogOpen || isPressed) { return; }

        isPressed = true;

        SoundManager.Instance.PlaySe("Press");

        StageSelectModel.inStageNum = model.CurrentState;

        string sceneName = model.GetSceneName();
        StageSelectManager.Instance.StageSelected(sceneName);
    }

    void TriggerPressedReturn()
    {
        isDialogOpen = true;

        confirmDialogPresenter.Open(
            onYes: () => StageSelectManager.Instance.ReturnTitle(),
            onNo: () =>
            {
                confirmDialogPresenter.Close();
                isDialogOpen = false;
            });
    }

    void ShowHighScores()
    {
        for (int i = 0; i < model.GetStageCount() - 1; i++)
        {
            int highScore = SaveManager.Instance.LoadHighScore(i);

            view.SetHighScore(i, highScore);
        }
    }

    void ShowBestRanks()
    {
        for (int i = 0; i < model.GetStageCount() - 1; i++)
        {
            int bestRank = SaveManager.Instance.LoadBestRank(i);

            view.SetBestRank(i, bestRank);
        }
    }

    void OnDisable()
    {
        view.OnPressedLeft -= TriggerPressedLeft;
        view.OnPressedRight -= TriggerPressedRight;
        view.OnPressedDecision -= TriggerPressedDecision;
        view.OnPressedReturn -= TriggerPressedReturn;
    }
}
