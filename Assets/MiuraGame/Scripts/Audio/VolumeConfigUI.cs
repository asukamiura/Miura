using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class VolumeConfigUI : MonoBehaviour
{
    [SerializeField] Slider masterSlider;
    [SerializeField] Slider bgmSlider;
    [SerializeField] Slider seSlider;

    public void SetMasterVolume(float masterVolume)
    {
        masterSlider.value = masterVolume;
    }

    public void SetBGMVolume(float bgmVolume)
    {
        bgmSlider.value = bgmVolume;
    }

    public void SetSeVolume(float seVolume) 
    {
        seSlider.value = seVolume;
    }

    // スライダーに変更があったら値を反映させる(イベント)
    public void SetMasterSliderEvent(UnityAction<float> sliderCallback)
    {
        SetValueChangedEvent(masterSlider, sliderCallback);
    }
    public void SetBGMSliderEvent(UnityAction<float> sliderCallback)
    {
        SetValueChangedEvent(bgmSlider, sliderCallback);
    }
    public void SetSESliderEvent(UnityAction<float> sliderCallback)
    {
        SetValueChangedEvent(seSlider, sliderCallback);
    }
    void SetValueChangedEvent(Slider slider, UnityAction<float> sliderCallback)
    {
        if (slider.onValueChanged != null)
        {
            slider.onValueChanged.RemoveAllListeners();
        }
        slider.onValueChanged.AddListener(sliderCallback);
    }
}
