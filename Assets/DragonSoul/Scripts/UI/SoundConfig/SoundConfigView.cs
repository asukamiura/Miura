using System;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class SoundConfigView : MonoBehaviour
{
    InputReceiver Input => InputReceiver.Instance;

    [SerializeField] GameObject[] buttons;
    [SerializeField] GameObject selectArrow;
    [SerializeField] Slider[] volumeSliders;

    public Action OnPressedUp;
    public Action OnPressedDown;
    public Action OnPressedLeft;
    public Action OnPressedRight;
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

    /// <summary>
    /// 現在の音量にスライダーの数値を合わせる
    /// </summary>
    /// <param name="masterVol">マスター音量</param>
    /// <param name="bgmVol">BGM音量</param>
    /// <param name="seVol">SE音量</param>
    public void SetCurrentVolume(float masterVol, float bgmVol, float seVol)
    {
        for (int i = 0; i < volumeSliders.Length; i++)
        {
            switch (i)
            {
                case 0:
                    volumeSliders[i].value = masterVol;
                    break;
                case 1:
                    volumeSliders[i].value = bgmVol;
                    break;
                case 2:
                    volumeSliders[i].value = seVol;
                    break;
            }
        }
    }

    public Slider GetSlider(int currentState)
    {
        return volumeSliders[currentState];
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
