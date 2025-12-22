using System;
using UnityEngine;

public class TitleView : MonoBehaviour
{
    InputReciver Input => InputReciver.Instance;

    [SerializeField] GameObject[] buttons;
    [SerializeField] GameObject selectArrow;

    public Action OnPressedUp;
    public Action OnPressedDown;
    public Action OnPressedDecision;
    public Action OnPressedClose;

    void Update()
    {
        if (Input.SelectMoveUp)
        {
            OnPressedUp?.Invoke();
        }

        if (Input.SelectMoveDown)
        {
            OnPressedDown?.Invoke();
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
