using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public enum ProfileNum { Normal, Ultimate, JustDodge, JustGuard, Clear }

public class PostEffectManager : MonoBehaviour
{
    [SerializeField] Volume volume1;
    [SerializeField] Volume volume2;
    [SerializeField] VolumeProfile normalProfile;
    [SerializeField] VolumeProfile ultimateProfile;
    [SerializeField] VolumeProfile justDodgeProfile;
    [SerializeField] VolumeProfile justGuardProfile;
    [SerializeField] VolumeProfile clearProfile;

    Volume activeVolume;
    Volume inactiveVolume;

   

    readonly Dictionary<ProfileNum, VolumeProfile> volumeProfileData = new Dictionary<ProfileNum, VolumeProfile>();

    public static PostEffectManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
            return;
        }
    }

    void Start()
    {
        activeVolume = volume1;
        inactiveVolume = volume2;

        volumeProfileData[ProfileNum.Normal] = normalProfile;
        volumeProfileData[ProfileNum.Ultimate] = ultimateProfile;
        volumeProfileData[ProfileNum.JustDodge] = justDodgeProfile;
        volumeProfileData[ProfileNum.JustGuard] = justGuardProfile;
        volumeProfileData[ProfileNum.Clear] = clearProfile; 

        activeVolume.profile = normalProfile;
    }

    public void ChangePostEffect(ProfileNum profileNum, float duration)
    {
        StartCoroutine(FadeProfile(profileNum, duration));
    }

    IEnumerator FadeProfile(ProfileNum profileNum, float duration)
    {
        inactiveVolume.profile = activeVolume.profile;
        activeVolume.profile = volumeProfileData[profileNum];

        float time = 0;

        while (time < duration)
        {
            float t = time / duration;
            inactiveVolume.weight = 1 - t;
            activeVolume.weight = t;

            time += Time.deltaTime;
            yield return null;
        }

        inactiveVolume.weight = 0;
        activeVolume.weight = 1;
    }
}
