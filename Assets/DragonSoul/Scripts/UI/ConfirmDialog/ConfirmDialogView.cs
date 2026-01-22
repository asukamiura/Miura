using System;
using UnityEngine;

public class ConfirmDialogView : MonoBehaviour
{
    InputReceiver Input => InputReceiver.Instance;

    [SerializeField] GameObject[] buttons;
    [SerializeField] GameObject selectArrow;

    public Action OnPressedLeft;
    public Action OnPressedRight;
    public Action OnPressedDecision;
    public Action OnPressedClose;

    void Update()
    {
        if (Input.SelectMoveLeft)
        {
            OnPressedLeft?.Invoke();
        }

        if (Input.SelectMoveRight)
        {
            OnPressedRight?.Invoke();
        }

        if (Input.Decision)
        {
            OnPressedDecision?.Invoke();
        }

        if (Input.Return)
        {
            OnPressedClose?.Invoke();
        }
    }

    public void MoveSelectArrow(int index)
    {
        selectArrow.transform.position = buttons[index].transform.position;
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
