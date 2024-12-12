using UnityEngine;
using UnityEngine.EventSystems;

public enum GameState { Playing, Paused, GameOver}
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameState CurrentState {  get; private set; } = GameState.Playing;
    [SerializeField] private GameObject savePrefab;
    private GameObject saveObj;
    private SaveManager saveManager;
    InputReciver Input => InputReciver.Instance;

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
        if (Input.Pause)
        {
            ChangeState(GameState.Paused);
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
}
