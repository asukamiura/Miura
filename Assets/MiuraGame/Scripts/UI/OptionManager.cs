using SoundSystem;
using UnityEngine;
using UnityEngine.UI;

public class OptionManager : MonoBehaviour
{
    [SerializeField] VolumeConfigUI volumeConfigUI;
    [SerializeField] Slider[] volumeSliders;
    [SerializeField] GameObject[] buttons;
    [SerializeField] GameObject selectArrow;
    [SerializeField] GameObject previousPanel;

    InputReciver Input => InputReciver.Instance;
    enum OptionPanelState { Master = 0, BGM, SE, Close }
    OptionPanelState optionState = OptionPanelState.Master;
    enum CurrentScene { Title, Main }
    float masterVol, bgmVol, seVol = 0;

    void Start()
    {
        gameObject.SetActive(false);
        SetVolume();
        
        SaveManager.Instance.LoadAudio(ref masterVol, ref bgmVol, ref seVol);

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

    void Update()
    {
        if (optionState != OptionPanelState.Close)
        {
            // ボリュームを変更
            if (Input.SelectMoveLeft)
            {
                volumeSliders[(int)optionState].value -= 0.1f;
            }
            else if (Input.SelectMoveRight)
            {
                volumeSliders[(int)optionState].value += 0.1f;
            }
        }

        // 選択中のボタンを変更
        if (Input.SelectMoveUp && optionState != OptionPanelState.Master)
        {
            optionState--;
            MoveSelectArrow();
            SoundManager.Instance.PlaySe("MenuMove");
        }
        else if (Input.SelectMoveDown && optionState != OptionPanelState.Close)
        {
            optionState++;
            MoveSelectArrow();
            SoundManager.Instance.PlaySe("MenuMove");
        }

        if (Input.Decision && optionState == OptionPanelState.Close)
        {
            SoundManager.Instance.PlaySe("Press");
            gameObject.SetActive(false);
            previousPanel.SetActive(true);
            SaveManager.Instance.SaveAudio(volumeSliders[0].value, volumeSliders[1].value, volumeSliders[2].value);
        }

        if (Input.Return)
        {
            SoundManager.Instance.PlaySe("Press");
            gameObject.SetActive(false);
            previousPanel.SetActive(true);
            SaveManager.Instance.SaveAudio(volumeSliders[0].value, volumeSliders[1].value, volumeSliders[2].value);
        }
    }

    void MoveSelectArrow()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (i == (int)optionState)
            {
                selectArrow.transform.position = buttons[i].transform.position;
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

    void OnEnable()
    {
        optionState = OptionPanelState.Master;
        MoveSelectArrow();
    }
}
