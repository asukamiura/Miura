using SoundSystem;
using UnityEngine;

public class TitleManager : MonoBehaviour
{
    [SerializeField] GameObject savePrefab;

    GameObject saveObj;
    SaveManager saveManager;

    const float FadeTime = 1.0f;

    public static TitleManager Instance { get; private set; }

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

        saveObj = Instantiate(savePrefab);
        saveObj.name = "SaveManager";       // そのままだと(Clone)がつくので名前上書き
        saveManager = saveObj.GetComponent<SaveManager>();
        if (!saveManager.SaveDataCheck())
        {
            saveManager.CreateSaveData();
        }
    }

    void Start()
    {
        StageSelectModel.inStageNum = 0;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        SoundManager.Instance.PlayBGMWithFadeIn("Title");
    }

    public void TransitionToSelect()
    {
        SoundManager.Instance.StopBGMWithFadeOut(FadeTime);
        FadeManager.Instance.LoadScene("SelectScene", FadeTime);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // ゲームプレイ終了
#else
            Application.Quit(); // ゲームプレイ終了
#endif
    }
}
