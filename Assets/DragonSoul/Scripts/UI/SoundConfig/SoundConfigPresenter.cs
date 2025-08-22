using SoundSystem;
using System;
using UnityEngine;

public class SoundConfigPresenter : MonoBehaviour
{
    [SerializeField] VolumeConfigUI volumeConfigUI;
    [SerializeField] SoundConfigView view;
    SoundConfigModel model;
    bool isPressed = false;
    float masterVol, bgmVol, seVol = 0;

    public Action OnClose;

    void Awake()
    {
        model = new SoundConfigModel();
    }

    void Start()
    {
        SetVolume();

        view.Hide();
        SaveManager.Instance.LoadAudio(ref masterVol, ref bgmVol, ref seVol);

        view.SetCurrentVolume(masterVol, bgmVol, seVol);
    }

    void OnEnable()
    {
        view.OnPressedUp += TriggerPressedUp;
        view.OnPressedDown += TriggerPressedDown;
        view.OnPressedLeft += TriggerPressedLeft;
        view.OnPressedRight += TriggerPressedRight;
        view.OnPressedDecision += TriggerPressedDecision;
        view.OnPressedClose += TriggerPressedClose;
    }

    void OnDisable()
    {
        view.OnPressedUp -= TriggerPressedUp;
        view.OnPressedDown -= TriggerPressedDown;
        view.OnPressedLeft -= TriggerPressedLeft;
        view.OnPressedRight -= TriggerPressedRight;
        view.OnPressedDecision -= TriggerPressedDecision;
        view.OnPressedClose -= TriggerPressedClose;
    }

    void TriggerPressedUp()
    {
        if (isPressed || model.CurrentState == SoundConfigModel.SoundConfigState.Master) { return; }

        SoundManager.Instance.PlaySe("MenuMove");
        model.ChangeStateUp();
        view.MoveSelectArrow((int)model.CurrentState);
    }

    void TriggerPressedDown()
    {
        if (isPressed || model.CurrentState == SoundConfigModel.SoundConfigState.Close) { return; }

        SoundManager.Instance.PlaySe("MenuMove");
        model.ChangeStateDown();
        view.MoveSelectArrow((int)model.CurrentState);
    }

    void TriggerPressedLeft()
    {
        if (model.CurrentState == SoundConfigModel.SoundConfigState.Close) { return; }

        model.ReduceVolume(view.GetSlider((int)model.CurrentState));
    }

    void TriggerPressedRight()
    {
        if (model.CurrentState == SoundConfigModel.SoundConfigState.Close) { return; }

        model.IncreaseVolume(view.GetSlider((int)model.CurrentState));
    }

    void TriggerPressedDecision()
    {
        if (isPressed || model.CurrentState != SoundConfigModel.SoundConfigState.Close) { return; }

        isPressed = true;

        SoundManager.Instance.PlaySe("Press");

        switch (model.CurrentState)
        {
            case SoundConfigModel.SoundConfigState.Close:
                TriggerPressedClose();
                break;
        }
    }

    void TriggerPressedClose()
    {
        SaveManager.Instance.SaveAudio(view.GetSlider(0).value, view.GetSlider(1).value, view.GetSlider(2).value);
        view.Hide();
        OnClose?.Invoke();
    }

    public void Open(Action onClose)
    {
        OnClose = onClose;

        isPressed = false;
        model.SetInitialState();
        view.MoveSelectArrow((int)model.CurrentState);
        view.Show();
    }

    public void SetVolume()
    {
        // ボリュームの設定
        volumeConfigUI.SetMasterSliderEvent(vol => SoundManager.Instance.MasterVolume = vol);
        volumeConfigUI.SetBGMSliderEvent(vol => SoundManager.Instance.BGMVolume = vol);
        volumeConfigUI.SetSESliderEvent(vol => SoundManager.Instance.SEVolume = vol);

        // スライダーの数値反映
        volumeConfigUI.SetMasterVolume(SoundManager.Instance.MasterVolume);
        volumeConfigUI.SetBGMVolume(SoundManager.Instance.BGMVolume);
        volumeConfigUI.SetSEVolume(SoundManager.Instance.SEVolume);
    }
}
