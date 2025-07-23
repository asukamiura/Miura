using System;
using UnityEngine;

public class ConfirmDialogView : MonoBehaviour
{
    InputReciver Input => InputReciver.Instance;

    [SerializeField] GameObject[] buttons;
    [SerializeField] GameObject selectArrow;

    public Action OnPressedLeft;
    public Action OnPressedRight;
    public Action OnPressedDecision;

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
    }

    public void MoveSelectArrow(int index)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (i == index)
            {
                selectArrow.transform.position = buttons[i].transform.position;
            }
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public bool ConfirmDialogEnabled()
    {
        return gameObject.activeSelf;
    }
}
