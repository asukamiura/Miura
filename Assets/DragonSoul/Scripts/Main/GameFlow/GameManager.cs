using Player;
using SoundSystem;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    //[SerializeField] PlayerCore playerCore;
    [SerializeField] EnemyCoreBase enemyCore;
    [SerializeField] HealthManager playerHealthManager;
    [SerializeField] HealthManager enemyHealthManager;
    [SerializeField] GameObject blackCurtain;
    [SerializeField] GameObject pausePanel;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject operationUI;

    InputReciver Input => InputReciver.Instance;
    bool isChangedScene = false;    // シーン遷移が実行されたかどうか
    float previousTimeScale = 1;
    bool previousEnabled = true;

    const float TransitionTime = 3;    // シーン遷移が起こるまでの待機時間
    const float GameStartTime = 8.3f;

    public static GameManager Instance { get; set; }
    public enum GameState { GameStart, Playing, Pause, GameOver }     // ゲームの状態
    public GameState CurrentState { get; set; } = GameState.GameStart;    // 現在の状態

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
        //StartCoroutine(Initialize());
        Time.timeScale = 1;
    }

    void Update()
    {
        //if (Input.Pause && CurrentState == GameState.Playing)
        //{
        //    ChangeState(GameState.Pause);
        //}

        //// プレイヤーが死んだらゲームオーバーステートに遷移
        //if (playerHealthManager.IsDead && CurrentState != GameState.GameOver && CurrentState == GameState.Playing)
        //{
        //    ChangeState(GameState.GameOver);
        //}

        //// 敵が死んだらリザルトシーンに遷移
        //if (enemyHealthManager.IsDead && !isChangedScene)
        //{
        //    isChangedScene = true;
        //    StartCoroutine(ChangeScene(TransitionTime));
        //}
    }

    // 各ステートに変更時実行
    public void ChangeState(GameState state)
    {
        CurrentState = state;
        switch (state)
        {
            case GameState.GameStart:
                //playerCore.enabled = false;
                CameraManager.Instance.IsInput = false;
                enemyCore.MoveActive(false);
                break;
            case GameState.Playing:
                blackCurtain.SetActive(false);
                operationUI.SetActive(false);
                //playerCore.enabled = previousEnabled;
                CameraManager.Instance.IsInput = true;
                enemyCore.MoveActive(true);
                Time.timeScale = previousTimeScale;
                break;
            case GameState.Pause:
                //previousEnabled = playerCore.enabled;
                //playerCore.enabled = false;
                blackCurtain.SetActive(true);
                operationUI.SetActive(true);
                //pausePanel.SetActive(true);
                previousTimeScale = Time.timeScale;
                Time.timeScale = 0;
                break;
            case GameState.GameOver:
                //StartCoroutine(ShowGameOverPanel());
                break;
        }
    }

    //IEnumerator ChangeScene(float delay)
    //{
    //    yield return new WaitForSeconds(delay);
    //    SoundManager.Instance.StopBGMWithFadeOut("Main", FadeTime);
    //    FadeManager.Instance.LoadScene("ResultScene", FadeTime);
    //}

    //IEnumerator Initialize()
    //{
    //    ChangeState(GameState.GameStart);

    //    yield return new WaitForSeconds(GameStartTime);

    //    ChangeState(GameState.Playing);
    //}

    //IEnumerator ShowGameOverPanel()
    //{
    //    yield return new WaitForSeconds(2);

    //    gameOverPanel.SetActive(true);
    //    Time.timeScale = 0;
    //}
}
