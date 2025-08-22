using SoundSystem;
using System;
using UnityEngine;

public class ConfirmDialogPresenter : MonoBehaviour
{
    [SerializeField] ConfirmDialogView view;
    ConfirmDialogModel model;
    bool isPressed = false;

    public Action OnYes;
    public Action OnNo;

    void Awake()
    {
        model = new ConfirmDialogModel();        
    }

    void Start()
    {
        view.Hide();
    }

    void OnEnable()
    {
        view.OnPressedLeft += TriggerPressedLeft;
        view.OnPressedRight += TriggerPressedRight;
        view.OnPressedDecision += TriggerPressedDecision;
        view.OnPressedClose += TriggerPressedClose;
    }

    void OnDisable()
    {
        view.OnPressedLeft -= TriggerPressedLeft;
        view.OnPressedRight -= TriggerPressedRight;
        view.OnPressedDecision -= TriggerPressedDecision;
        view.OnPressedClose -= TriggerPressedClose;
    }

    void TriggerPressedLeft()
    {
        if (isPressed || model.CurrentState == ConfirmDialogModel.ConfirmState.Yes) { return; }

        SoundManager.Instance.PlaySe("MenuMove");
        model.ChangeStateLeft();
        view.MoveSelectArrow((int)model.CurrentState);
    }

    void TriggerPressedRight()
    {
        if (isPressed || model.CurrentState == ConfirmDialogModel.ConfirmState.No) { return; }

        SoundManager.Instance.PlaySe("MenuMove");
        model.ChangeStateRight();
        view.MoveSelectArrow((int)model.CurrentState);
    }

    void TriggerPressedDecision()
    {
        if (isPressed) { return; }

        isPressed = true;

        SoundManager.Instance.PlaySe("Press");

        switch (model.CurrentState)
        {
            case ConfirmDialogModel.ConfirmState.Yes:
                OnYes?.Invoke();
                break;
            case ConfirmDialogModel.ConfirmState.No:
                TriggerPressedClose();
                break;
        }
    }

    void TriggerPressedClose()
    {
        view.Hide();
        OnNo?.Invoke();
    }

    public void Open(Action onYes, Action onNo)
    {
        OnYes = onYes;
        OnNo = onNo;

        isPressed = false;
        model.SetInitialState();
        view.MoveSelectArrow((int)model.CurrentState);
        view.Show();
    }
}
