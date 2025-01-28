using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

namespace SoundSystem
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance { get; set; }

        // BGM・SEのAudioClipリスト
        public List<AudioClip> bgmAudioClipList = new List<AudioClip>();
        public List<AudioClip> seAudioClipList = new List<AudioClip>();

        [SerializeField, Header("Audio Mixer")]
        public AudioMixer audioMixer;
        public AudioMixerGroup bgmAMG, seAMG;
        public AudioMixer effectAudioMixer;

        // BGM・SEの各AudioSourace
        List<AudioSource> bgmAudioSourceList = new List<AudioSource>();
        List<AudioSource> seAudioSourceList = new List<AudioSource>();

        List<IEnumerator> fadeCoroutines = new List<IEnumerator>();

        const int BGMAudioSourceNum = 2;
        const string MasterVolumeParamName = "MasterVolume";
        const string SEVolumeParamName = "SEVolume";
        const string BGMVolumeParamName = "BGMVolume";
        const int SEAudioSourceNum = 10;

        // 一時停止中か
        public bool IsPaused { get; set; }

        public float MasterVolume
        {
            get { return audioMixer.GetVolumeByLinear(MasterVolumeParamName); }
            set { audioMixer.SetVolumeByLinear(MasterVolumeParamName, value); }
        }
        public float SEVolume
        {
            get { return audioMixer.GetVolumeByLinear(SEVolumeParamName); }
            set { audioMixer.SetVolumeByLinear(SEVolumeParamName, value); }
        }
        public float BGMVolume
        {
            get { return audioMixer.GetVolumeByLinear(BGMVolumeParamName); }
            set { audioMixer.SetVolumeByLinear(BGMVolumeParamName, value); }
        }

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(this);
                return;
            }
            seAudioSourceList = InitializeAudioSources(gameObject, false, seAMG, SEAudioSourceNum); bgmAudioSourceList = InitializeAudioSources(gameObject, true, bgmAMG, BGMAudioSourceNum);

            IsPaused = false;
        }

        public void ChangeSnapshot(string snapshotName, float transitionTime = 1f)
        {
            AudioMixerSnapshot snapshot = effectAudioMixer.FindSnapshot(snapshotName);

            if (snapshot == null)
            {
                Debug.Log(snapshotName + "は見つかりません");
            }
            else
            {
                snapshot.TransitionTo(transitionTime);
            }
        }

        public void Pause()
        {
            IsPaused = true;

            fadeCoroutines.ForEach(StopCoroutine);

            bgmAudioSourceList.ForEach(bas => bas.Pause());
        }

        public void Resume()
        {
            IsPaused = false;

            fadeCoroutines.ForEach(routine => StopCoroutine(routine));

            bgmAudioSourceList.ForEach(bas => bas.UnPause());
        }

        List<AudioSource> InitializeAudioSources(GameObject parentGameObject, bool isLoop = false,
           AudioMixerGroup amg = null, int count = 1)
        {
            List<AudioSource> audioSources = new List<AudioSource>();

            for (int i = 0; i < count; i++)
            {
                var audioSource = InitializeAudioSource(parentGameObject, isLoop, amg);
                audioSources.Add(audioSource);
            }

            return audioSources;
        }

        AudioSource InitializeAudioSource(GameObject parentGameObject, bool isLoop = false,
           AudioMixerGroup amg = null)
        {
            var audioSource = parentGameObject.AddComponent<AudioSource>();

            audioSource.loop = isLoop;
            audioSource.playOnAwake = false;

            if (amg != null)
            {
                audioSource.outputAudioMixerGroup = amg;
            }

            return audioSource;
        }

        public void PlaySe(string clipName)
        {
            var audioClip = seAudioClipList.FirstOrDefault(clip => clip.name == clipName);

            if (audioClip == null)
            {
                Debug.Log(clipName + "を見つかりません");
                return;
            }

            // 利用可能なAudioSourceを取得
            AudioSource audioSource = seAudioSourceList.FirstOrDefault(asb => !asb.isPlaying);

            if (audioSource != null)
            {
                audioSource.clip = audioClip;
                audioSource.Play();
            }
            else
            {
                Debug.Log("利用可能なAudioSourceがありません");
            }
        }

        public void PlayBGMWithFadeIn(string clipName, float fadeTime = 2f)
        {
            if (IsPaused) { return; }

            var audioClip = bgmAudioClipList.FirstOrDefault(clip => clip.name == clipName);

            if (audioClip == null)
            {
                Debug.Log(clipName + "は見つかりません");
                return;
            }

            if (bgmAudioSourceList.Any(source => source.clip == audioClip))
            {
                Debug.Log(clipName + "はすでに再生されています");
                return;
            }

            StopBGMWithFadeOut(fadeTime);

            AudioSource audioSource = bgmAudioSourceList.FirstOrDefault(asb => asb.isPlaying == false);

            if (audioSource != null)
            {
                IEnumerator routine = audioSource.PlayWithFadeIn(audioClip, fadeTime);
                fadeCoroutines.Add(routine);
                StartCoroutine(routine);
            }
        }

        public void StopBGMWithFadeOut(string clipName, float fadeTime = 2f)
        {
            if (IsPaused) { return; }

            AudioSource audioSource = bgmAudioSourceList.FirstOrDefault(bas => bas.clip.name == clipName);

            if (audioSource == null || audioSource.isPlaying == false)
            {
                Debug.Log(clipName + "再生されていません");
                return;
            }

            IEnumerator routine = audioSource.StopWithFadeOut(fadeTime);
            StartCoroutine(routine);
            fadeCoroutines.Add(routine);
        }

        public void StopBGMWithFadeOut(float fadeTime = 2f)
        {
            if (IsPaused) { return; }

            fadeCoroutines.ForEach(StopCoroutine);
            fadeCoroutines.Clear();

            fadeCoroutines = bgmAudioSourceList.Where(asb => asb.isPlaying)
                .ToList()
                .ConvertAll(asb =>
                {
                    IEnumerator routine = asb.StopWithFadeOut(fadeTime);
                    StartCoroutine(routine);
                    return routine;
                });
        }
    }
}