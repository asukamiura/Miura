using UnityEngine;

public class FrameRateLimiter : MonoBehaviour
{
    static FrameRateLimiter instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        Application.targetFrameRate = 30;
        QualitySettings.vSyncCount = 0;
    }
}
