using SoundSystem;
using System;
using UnityEngine;

public class PauseMenuPresenter : MonoBehaviour
{
    [SerializeField] PauseMenuView pauseMenuView;
    [SerializeField] ConfirmDialogPresenter confirmDialogPresenter;
    [SerializeField] SoundConfigPresenter soundConfigPresenter;
    PauseMenuModel model;
    bool isOpenDialog = false;
    bool isPressed = false;

    Action OnClose;

    void Awake()
    {
        model = new PauseMenuModel();
    }

    void Start()
    {
        pauseMenuView.Hide();
    }

    void OnEnable()
    {
        pauseMenuView.OnPressedUp += TriggerPressedUp;
        pauseMenuView.OnPressedDown += TriggerPressedDown;
        pauseMenuView.OnPressedDecision += TriggerPressedDecision;
        pauseMenuView.OnPressedClose += TriggerPressedClose;
    }

    void OnDisable()
    {
        pauseMenuView.OnPressedUp -= TriggerPressedUp;
        pauseMenuView.OnPressedDown -= TriggerPressedDown;
        pauseMenuView.OnPressedDecision -= TriggerPressedDecision;
        pauseMenuView.OnPressedClose -= TriggerPressedClose;
    }


    void TriggerPressedUp()
    {
        if (isOpenDialog || isPressed || model.CurrentState == PauseMenuModel.PauseMenuState.ReturnSelect) { return; }

        SoundManager.Instance.PlaySe("MenuMove");
        model.ChangeStateUp();
        pauseMenuView.MoveSelectArrow((int)model.CurrentState);
    }

    void TriggerPressedDown()
    {
        if (isOpenDialog || isPressed || model.CurrentState == PauseMenuModel.PauseMenuState.Close) { return; }
 
        SoundManager.Instance.PlaySe("MenuMove");
        model.ChangeStateDown();
        pauseMenuView.MoveSelectArrow((int)model.CurrentState);
    }

    void TriggerPressedDecision()
    {
        if (isOpenDialog || isPressed) { return; }

        isPressed = true;

        SoundManager.Instance.PlaySe("Press");

        switch (model.CurrentState)
        {
            case PauseMenuModel.PauseMenuState.ReturnSelect:
                pauseMenuView.Hide();
                OpenConfirmDialog();
                break;
            case PauseMenuModel.PauseMenuState.Option:
                pauseMenuView.Hide();
                OpenSoundConfig();
                break;
            case PauseMenuModel.PauseMenuState.Close:
                TriggerPressedClose();
                break;
        }
    }

    void TriggerPressedClose()
    {
        OnClose?.Invoke();        
    }

    void OpenConfirmDialog()
    {
        isOpenDialog = true;

        confirmDialogPresenter.Open(
            onYes: () => GameFlowManagerBase.Instance.TransitionToSelectScenen(),
            onNo: () =>
            {
                isOpenDialog = false;
                isPressed = false;
                pauseMenuView.Show();
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
                pauseMenuView.Show();
            });
    }

    public void Open(Action onClose)
    {
        OnClose = onClose;

        isOpenDialog = false;
        isPressed = false;
        model.SetInitialState();
        pauseMenuView.MoveSelectArrow((int)model.CurrentState);
        pauseMenuView.Show();
    }

    public void Close()
    {
        pauseMenuView.Hide();
    }
}
