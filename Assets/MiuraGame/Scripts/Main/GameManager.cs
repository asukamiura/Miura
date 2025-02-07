using Player;
using SoundSystem;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject savePrefab;
    [SerializeField] PlayerCore playerCore;
    [SerializeField] HealthManager playerHealthManager;
    [SerializeField] HealthManager enemyHealthManager;
    [SerializeField] GameObject blackCurtain;
    [SerializeField] GameObject pausePanel;
    [SerializeField] GameObject gameOverPanel;

    GameObject saveObj;
    SaveManager saveManager;
    InputReciver Input => InputReciver.Instance;
    bool isChangedScene = false;    // シーン遷移が実行されたかどうか
    const float TransitionTime = 3;    // シーン遷移が起こるまでの待機時間

    public static GameManager Instance { get; set; }
    public enum GameState { Playing, Paused, GameOver }     // ゲームの状態
    public GameState CurrentState { get; set; } = GameState.Playing;    // 現在の状態

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        saveObj = Instantiate(savePrefab);
        saveObj.name = "SaveManager";
        saveManager = saveObj.GetComponent<SaveManager>();
    }

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        SoundManager.Instance.PlayBGMWithFadeIn("Main");
        blackCurtain.SetActive(false);
    }

    void Update()
    {
        if (Input.Pause && CurrentState == GameState.Playing)
        {
            ChangeState(GameState.Paused);
        }

        // プレイヤーが死んだらゲームオーバーステートに遷移
        if (playerHealthManager.IsDead && CurrentState != GameState.GameOver)
        {
            ChangeState(GameState.GameOver);
        }

        // 敵が死んだらリザルトシーンに遷移
        if (enemyHealthManager.IsDead && !isChangedScene)
        {
            isChangedScene = true;
            StartCoroutine(ChangeScene(TransitionTime));
        }
    }

    // 各ステートに変更時実行
    public void ChangeState(GameState state)
    {
        CurrentState = state;
        switch (state)
        {
            case GameState.Playing:
                blackCurtain.SetActive(false);
                playerCore.enabled = true;
                break;
            case GameState.Paused:
                playerCore.enabled = false;
                Time.timeScale = 0;
                blackCurtain.SetActive(true);
                pausePanel.SetActive(true);
                break;
            case GameState.GameOver:
                gameOverPanel.SetActive(true);
                break;
        }
    }

    IEnumerator ChangeScene(float delay)
    {
        yield return new WaitForSeconds(delay);
        SoundManager.Instance.StopBGMWithFadeOut();
        FadeManager.Instance.LoadScene("ResultScene");
    }
}
