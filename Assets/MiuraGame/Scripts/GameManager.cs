using UnityEngine;
using Player;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject savePrefab;
    [SerializeField] private HealthManager playerHealthManager;
    [SerializeField] private HealthManager enemyHealthManager;

    private GameObject saveObj;
    private SaveManager saveManager;
    private InputReciver Input => InputReciver.Instance;
    private float delayTime = 3;    // シーン遷移が起こるまでの待機時間

    public static GameManager Instance { get; private set; }
    public enum GameState { Playing, Paused, GameOver }     // ゲームの状態
    public GameState CurrentState { get; private set; } = GameState.Playing;    // 現在の状態

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
    }

    void Update()
    {
        //if (Input.Pause)
        //{
        //    ChangeState(GameState.Paused);
        //}

        // プレイヤーが死んだらゲームオーバーステートに遷移
        if (playerHealthManager.isDead)
        {
            ChangeState(GameState.GameOver);            
        }

        // 敵が死んだらリザルトシーンに遷移
        if (enemyHealthManager.isDead)
        {
            StartCoroutine(ChangeScene(delayTime));
        }
    }

    private void ChangeState(GameState state)
    {
        CurrentState = state;
        switch (state)
        {
            case GameState.Playing:
                break;
            case GameState.Paused:
                break;
            case GameState.GameOver:
                break;
        }
    }

    private IEnumerator ChangeScene(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("ResultScene");
    }
}
