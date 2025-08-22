using SoundSystem;
using UnityEngine;

public class GameOverPresenter : MonoBehaviour
{
    [SerializeField] GameOverView gameOverView;
    [SerializeField] ConfirmDialogPresenter confirmDialogPresenter;
    GameOverModel gameOverModel;
    bool isPressed = false;
    bool isOpenDialog = false;

    void Awake()
    {
        gameOverModel = new GameOverModel();
    }

    void Start()
    {
        gameOverView.Hide();
    }

    void OnEnable()
    {
        gameOverView.OnPressedLeft += TriggerPressedLeft;
        gameOverView.OnPressedRight += TriggerPressedRight;
        gameOverView.OnPressedDecision += TriggerPressedDecision;
    }

    void OnDisable()
    {
        gameOverView.OnPressedLeft -= TriggerPressedLeft;
        gameOverView.OnPressedRight -= TriggerPressedRight;
        gameOverView.OnPressedDecision -= TriggerPressedDecision;
    }

    void TriggerPressedLeft()
    {
        if (isPressed || isOpenDialog || gameOverModel.CurrentState == GameOverModel.GameOverState.ReturnSelect) { return; }

        SoundManager.Instance.PlaySe("MenuMove");
        gameOverModel.ChangeStateLeft();
        gameOverView.MoveSelectArrow((int)gameOverModel.CurrentState);
    }

    void TriggerPressedRight()
    {
        if (isPressed || isOpenDialog || gameOverModel.CurrentState == GameOverModel.GameOverState.Retry) { return; }

        SoundManager.Instance.PlaySe("MenuMove");
        gameOverModel.ChangeStateRight();
        gameOverView.MoveSelectArrow((int)gameOverModel.CurrentState);
    }

    void TriggerPressedDecision()
    {
        if (isPressed) { return; }

        isPressed = true;

        SoundManager.Instance.PlaySe("Press");

        switch (gameOverModel.CurrentState)
        {
            case GameOverModel.GameOverState.ReturnSelect:
                gameOverView.Hide();
                OpenConfirmDialog();
                break;
            case GameOverModel.GameOverState.Retry:
                GameFlowManagerBase.Instance.TransitionToCurrentScenen();
                break;
        }
    }

    void OpenConfirmDialog()
    {
        isOpenDialog = true;
        confirmDialogPresenter.Open(
            onYes: () => GameFlowManagerBase.Instance.TransitionToSelectScenen(),
            onNo: () =>
            {
                isOpenDialog = false;
                gameOverView.Show();
            });
    }

    public void Open()
    {
        isPressed = false;
        gameOverModel.SetInitialState();
        gameOverView.MoveSelectArrow((int)gameOverModel.CurrentState);
        gameOverView.Show();
    }
}
