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
        view.OnPressedLeft += TriggerPressedLeft;
        view.OnPressedRight += TriggerPressedRight;
        view.OnPressedDecision += TriggerPressedDecision;

        view.Hide();
    }

    void TriggerPressedLeft()
    {
        if (isPressed) { return; }

        model.ChangeStateLeft();
        view.MoveSelectArrow((int)model.CurrentState);
    }

    void TriggerPressedRight()
    {
        if (isPressed) { return; }

        model.ChangeStateRight();
        view.MoveSelectArrow((int)model.CurrentState);
    }

    void TriggerPressedDecision()
    {
        if (isPressed) { return; }

        isPressed = true;

        switch (model.CurrentState)
        {
            case ConfirmDialogModel.ConfirmState.Yes:
                OnYes?.Invoke();
                break;
            case ConfirmDialogModel.ConfirmState.No:
                view.Hide();
                OnNo?.Invoke();
                break;
        }
    }

    public void Open(Action onYes, Action onNo)
    {
        model.SetState();
        OnYes = onYes;
        OnNo = onNo;

        view.MoveSelectArrow((int)model.CurrentState);
        view.Show();
    }

    public void Close()
    {
        view.Hide();
    }
}
