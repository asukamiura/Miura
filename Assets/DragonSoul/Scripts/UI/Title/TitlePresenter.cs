using SoundSystem;
using UnityEngine;

public class TitlePresenter : MonoBehaviour
{
    [SerializeField] TitleView titleView;
    [SerializeField] ConfirmDialogPresenter confirmDialogPresenter;
    [SerializeField] SoundConfigPresenter soundConfigPresenter;
    TitleModel model;
    bool isOpenDialog = false;
    bool isPressed = false;

    void Awake()
    {
        model = new TitleModel();
    }

    void OnEnable()
    {
        titleView.OnPressedUp += TriggerPressedUp;
        titleView.OnPressedDown += TriggerPressedDown;
        titleView.OnPressedDecision += TriggerPressedDecision;
    }

    void OnDisable()
    {
        titleView.OnPressedUp -= TriggerPressedUp;
        titleView.OnPressedDown -= TriggerPressedDown;
        titleView.OnPressedDecision -= TriggerPressedDecision;
    }


    void TriggerPressedUp()
    {
        if (isOpenDialog || isPressed || model.CurrentState == TitleModel.TitleState.Start) { return; }

        SoundManager.Instance.PlaySe("MenuMove");
        model.ChangeStateUp();
        titleView.MoveSelectArrow((int)model.CurrentState);
    }

    void TriggerPressedDown()
    {
        if (isOpenDialog || isPressed || model.CurrentState == TitleModel.TitleState.Quit) { return; }

        SoundManager.Instance.PlaySe("MenuMove");
        model.ChangeStateDown();
        titleView.MoveSelectArrow((int)model.CurrentState);
    }

    void TriggerPressedDecision()
    {
        if (isOpenDialog || isPressed) { return; }

        isPressed = true;

        SoundManager.Instance.PlaySe("Press");

        switch (model.CurrentState)
        {
            case TitleModel.TitleState.Start:
                TitleManager.Instance.TransitionToSelect();
                break;
            case TitleModel.TitleState.Option:
                titleView.Hide();
                OpenSoundConfig();
                break;
            case TitleModel.TitleState.Quit:
                titleView.Hide();
                OpenConfirmDialog();
                break;
        }
    }

    void OpenConfirmDialog()
    {
        isOpenDialog = true;

        confirmDialogPresenter.Open(
            onYes: () => TitleManager.Instance.QuitGame(),
            onNo: () =>
            {
                isOpenDialog = false;
                isPressed = false;
                titleView.Show();
            });
    }

    void OpenSoundConfig()
    {
        isOpenDialog = true;

        soundConfigPresenter.Open(
            onClose: () =>
            {
                isOpenDialog = false;
                isPressed = false;
                titleView.Show();
            });
    }
}
