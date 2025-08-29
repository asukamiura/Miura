using SoundSystem;
using UnityEngine;

public class StageSelectManager : MonoBehaviour
{
    const float FadeTime = 1.0f;

    public static StageSelectManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    void Start()
    {
        SoundManager.Instance.PlayBGMWithFadeIn("Select");

        Time.timeScale = 1;
    }

    public void StageSelected(string sceneName)
    {
        SoundManager.Instance.StopBGMWithFadeOut(FadeTime);
        FadeManager.Instance.LoadScene(sceneName, FadeTime);
    }

    public void ReturnTitle()
    {
        SoundManager.Instance.StopBGMWithFadeOut(FadeTime);
        FadeManager.Instance.LoadScene("TitleScene", FadeTime);
    }
}
