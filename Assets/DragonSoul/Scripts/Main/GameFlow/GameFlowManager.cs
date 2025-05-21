using SoundSystem;
using UnityEngine;

public class GameFlowManager : MonoBehaviour
{
    [SerializeField] PausePanelManager pausePanelManager;
    [SerializeField] GameOverPanelManager gameOverManager;
    [SerializeField] HealthManager playerHealthManager;
    [SerializeField] HealthManager enemyHealthManager;
    [SerializeField] EnemyCoreBase enemyCore;
    [SerializeField] GameObject blackCurtain;
    [SerializeField] GameObject operationUI;
    [SerializeField] GameObject savePrefab;

    StateMachine<GameFlowStateID> stateMachine;
    GameObject saveObj;
    SaveManager saveManager;

    const float FadeTime = 1;

    public float PreviousTimeScale { get; set; } = 1;
    public static GameFlowManager Instance { get; private set; }

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

        stateMachine = new StateMachine<GameFlowStateID>();

        stateMachine.RegisterState(new IntroState(this, enemyCore));
        stateMachine.RegisterState(new PlayingState(this, playerHealthManager, enemyHealthManager));
        stateMachine.RegisterState(new PauseState(this, pausePanelManager));
        stateMachine.RegisterState(new ClearState(this));
        stateMachine.RegisterState(new GameOverState(this, gameOverManager));

        saveObj = Instantiate(savePrefab);
        saveObj.name = "SaveManager";
        saveManager = saveObj.GetComponent<SaveManager>();
    }

    private void Start()
    {
        stateMachine.Initialize(GameFlowStateID.Intro);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        SoundManager.Instance.PlayBGMWithFadeIn("Main", FadeTime);

        blackCurtain.SetActive(false);
        operationUI.SetActive(false);
    }

    void Update()
    {
        stateMachine.StateUpdate();

        Debug.Log(stateMachine.CurrentState);
    }

    void FixedUpdate()
    {
        stateMachine.StateUpdate();
    }

    public void ChangeState(GameFlowStateID targetState)
    {
        stateMachine.ChangeState(targetState);
    }

    public void ShowOperationUI()
    {
        operationUI.SetActive(true);
    }

    public void HideOperationUI()
    {
        operationUI.SetActive(false);
    }

    public void ShowBlackCurtain()
    {
        blackCurtain.SetActive(true);
    }

    public void HideBlackCurtain()
    {
        blackCurtain.SetActive(false);
    }
}
