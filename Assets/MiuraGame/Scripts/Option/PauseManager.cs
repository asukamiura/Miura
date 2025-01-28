using Player;
using SoundSystem;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    [SerializeField] PlayerCore playerCore;
    [SerializeField] VolumeConfigUI volumeConfigUI;
    [SerializeField] GameObject blackCurtain;
    [SerializeField] GameObject pausePanel;
    [SerializeField] GameObject volumePanel;
    [SerializeField] Slider[] volumeSliders;
    [SerializeField] TextMeshProUGUI[] pauseTexts;
    [SerializeField] TextMeshProUGUI[] volumeTexts;

    InputReciver Input => InputReciver.Instance;
    enum PausePanelState { ReturnSelect, Option, Close }
    PausePanelState pauseState = PausePanelState.Close;
    enum VolumePanelState { Master = 0, BGM, SE, Close }
    VolumePanelState volumeState = VolumePanelState.Master;

    void Start()
    {
        SetVolume();
        //SoundManager.Instance.PlayBGMWithFadeIn("Title");
        blackCurtain.SetActive(false);
        pausePanel.SetActive(false);
        volumePanel.SetActive(false);

        ChangePauseTextsColor();
        ChangeVolumeTextsColor();
    }

    void Update()
    {
        if (Input.Pause)
        {
            ChangePauseTextsColor();
            ChangeVolumeTextsColor();
            playerCore.enabled = false;
            blackCurtain.SetActive(true);
            pausePanel.SetActive(true);
            Time.timeScale = 0;
        }

        if (pausePanel.activeSelf && !volumePanel.activeSelf)
        {
            // 選択中のボタンを変更
            if (Input.SelectMoveUp && pauseState != PausePanelState.ReturnSelect)
            {
                pauseState--;
                ChangePauseTextsColor();
            }
            else if (Input.SelectMoveDown && pauseState != PausePanelState.Close)
            {
                pauseState++;
                ChangePauseTextsColor();
            }

            if (Input.Decision)
            {
                switch (pauseState)
                {
                    case PausePanelState.ReturnSelect:
                        Time.timeScale = 1;
                        SceneManager.LoadScene("SelectScene");
                        break;
                    case PausePanelState.Option:
                        pausePanel.SetActive(false);
                        volumePanel.SetActive(true);
                        break;
                    case PausePanelState.Close:
                        pausePanel.SetActive(false);
                        blackCurtain.SetActive(false);
                        Time.timeScale = 1;
                        playerCore.enabled = true;
                        break;
                }
            }
        }
        else if (!pausePanel.activeSelf && volumePanel.activeSelf)
        {
            if (volumeState != VolumePanelState.Close)
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
                ChangeVolumeTextsColor();
            }
            else if (Input.SelectMoveDown && volumeState != VolumePanelState.Close)
            {
                volumeState++;
                ChangeVolumeTextsColor();
            }

            if (Input.Decision && volumeState == VolumePanelState.Close)
            {
                volumePanel.SetActive(false);
                pausePanel.SetActive(true);
            }
        }
    }

    void ChangePauseTextsColor()
    {
        // テキストの色を変更
        for (int i = 0; i < pauseTexts.Length; i++)
        {
            if (i == (int)pauseState)
            {
                pauseTexts[i].color = Color.red;
            }
            else
            {
                pauseTexts[i].color = Color.black;
            }
        }
    }

    void ChangeVolumeTextsColor()
    {
        // テキストの色を変更
        for (int i = 0; i < volumeTexts.Length; i++)
        {
            if (i == (int)volumeState)
            {
                volumeTexts[i].color = Color.red;
            }
            else
            {
                volumeTexts[i].color = Color.black;
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
