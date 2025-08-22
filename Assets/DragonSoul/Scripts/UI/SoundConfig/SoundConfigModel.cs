using UnityEngine.UI;

public class SoundConfigModel
{
    public enum SoundConfigState { Master = 0, BGM, SE, Close }
    SoundConfigState currentState = SoundConfigState.Master;

    public SoundConfigState CurrentState => currentState;

    public void ChangeStateDown()
    {
        if (currentState < SoundConfigState.Close)
        {
            currentState++;
        }
    }

    public void ChangeStateUp()
    {
        if (currentState > SoundConfigState.Master)
        {
            currentState--;
        }
    }

    /// <summary>
    /// 音量を上げる
    /// </summary>
    /// <param name="volumeSlider">音量スライダー</param>
    public void IncreaseVolume(Slider volumeSlider)
    {
        if (currentState == SoundConfigState.Close) { return; }

        volumeSlider.value += 0.1f;
    }

    /// <summary>
    /// 音量を下げる
    /// </summary>
    /// <param name="volumeSlider">音量スライダー</param>
    public void ReduceVolume(Slider volumeSlider)
    {
        if (currentState == SoundConfigState.Close) { return; }

        volumeSlider.value -= 0.1f;
    }

    public void SetInitialState()
    {
        currentState = SoundConfigState.Master;
    }
}
