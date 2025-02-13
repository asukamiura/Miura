using Enemy;
using Player;
using SoundSystem;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject savePrefab;
    [SerializeField] PlayerCore playerCore;
    [SerializeField] PlayerCameraController playerCameraController;
    [SerializeField] EnemyCoreBase enemyCore;
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
    const float GameStartTime = 8.3f;

    public static GameManager Instance { get; set; }
    public enum GameState { GameStart, Playing, Paused, GameOver }     // ゲームの状態
    public GameState CurrentState { get; set; } = GameState.GameStart;    // 現在の状態

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

        StartCoroutine(Initialize());
        Time.timeScale = 1;
    }

    void Update()
    {       
        if (Input.Pause && CurrentState == GameState.Playing)
        {
            ChangeState(GameState.Paused);
        }

        // プレイヤーが死んだらゲームオーバーステートに遷移
        if (playerHealthManager.IsDead && CurrentState != GameState.GameOver && CurrentState == GameState.Playing)
        {
            ChangeState(GameState.GameOver);
        }

        // 敵が死んだらリザルトシーンに遷移
        if (enemyHealthManager.IsDead && !isChangedScene)
        {
            isChangedScene = true;
            StartCoroutine(ChangeScene(TransitionTime));
        }

        Debug.Log(CurrentState.ToString());
    }

    // 各ステートに変更時実行
    public void ChangeState(GameState state)
    {
        CurrentState = state;
        switch (state)
        {
            case GameState.GameStart:
                playerCore.enabled = false;
                playerCameraController.enabled = false;
                enemyCore.MoveActive(false);
                break;
            case GameState.Playing:
                blackCurtain.SetActive(false);
                playerCore.enabled = true;
                playerCameraController.enabled = true;
                enemyCore.MoveActive(true);
                break;
            case GameState.Paused:
                playerCore.enabled = false;
                blackCurtain.SetActive(true);
                pausePanel.SetActive(true);
                Time.timeScale = 0;
                break;
            case GameState.GameOver:
                gameOverPanel.SetActive(true);
                Time.timeScale = 0;
                break;
        }
    }

    IEnumerator ChangeScene(float delay)
    {
        yield return new WaitForSeconds(delay);
        SoundManager.Instance.StopBGMWithFadeOut();
        FadeManager.Instance.LoadScene("ResultScene");
    }

    IEnumerator Initialize()
    {
        ChangeState(GameState.GameStart);

        yield return new WaitForSeconds(GameStartTime);

        ChangeState(GameState.Playing);
    }
}
