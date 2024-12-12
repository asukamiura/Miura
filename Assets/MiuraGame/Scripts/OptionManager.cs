using SoundSystem;
using UnityEngine;
using UnityEngine.UI;

public class OptionManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject optionPanel;
    [SerializeField] private GameObject volumePanel;
    [SerializeField] private VolumeConfigUI volumeConfigUI;
    [SerializeField] private Slider[] volumeSliders;
    InputReciver Input => InputReciver.Instance;
    private enum VolumePanelState { Master = 0, BGM, SE, Colse}
    private VolumePanelState volumeState = VolumePanelState.Master;

    private void Start()
    {
        SetVolume();
        SoundManager.Instance.PlayBGMWithFadeIn("Title");
    }

    private void Update()
    {
        if (volumePanel.activeSelf)
        {
            if (volumeState != VolumePanelState.Colse)
            {
                // ボリュームを変更
                if (Input.SelectMoveLeft)
                {
                    volumeSliders[(int)volumeState].value -= 0.1f;
                }
                else if (Input.SelectMoveRight)
                {
                    volumeSliders[(int)volumeState].value += 0.1f;
                }
            }

            // 選択中のボタンを変更
            if (Input.SelectMoveUp && volumeState != VolumePanelState.Master)
            {
                volumeState--;
            }
            else if (Input.SelectMoveDown && volumeState != VolumePanelState.Colse)
            {
                volumeState++;
            }
        }
    }

    public void SetVolume()
    {
        // ボリュームの設定
        volumeConfigUI.SetMasterSliderEvent(vol => SoundManager.Instance.MasterVolume = vol);
        volumeConfigUI.SetBGMSliderEvent(vol => SoundManager.Instance.BGMVolume = vol);
        volumeConfigUI.SetSESliderEvent(vol => SoundManager.Instance.SEVolume = vol);

        // スライダーの数値反映
        volumeConfigUI.SetMasterVolume(SoundManager.Instance.MasterVolume);
        volumeConfigUI.SetBGMVolume(SoundManager.Instance.MasterVolume);
        volumeConfigUI.SetSEVolume(SoundManager.Instance.MasterVolume);
    }
}
