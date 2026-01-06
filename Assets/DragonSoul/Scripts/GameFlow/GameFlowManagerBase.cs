using SoundSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFlowManagerBase : MonoBehaviour
{
    [SerializeField] GameObject blackCurtain;
    [SerializeField] GameObject operationUI;
    [SerializeField] GameObject savePrefab;
    [SerializeField] protected HealthManager playerHealthManager;
    [SerializeField] protected HealthManager enemyHealthManager;
    [SerializeField] protected EnemyCoreBase enemyCore;
    [SerializeField] protected PauseMenuPresenter pauseMenuPresenter;
    [SerializeField] protected GameOverPresenter gameOverPresenter;
    [SerializeField] int stageNum = 0;

    protected StateMachine<GameFlowStateID> stateMachine;
    GameObject saveObj;
    SaveManager saveManager;

    const float FadeTime = 1;

    public GameFlowStateID CurrentState => stateMachine.CurrentState;
    public GameFlowStateID PreivousState => stateMachine.PreviousState;
    public float PreviousTimeScale { get; set; } = 1;

    public static GameFlowManagerBase Instance { get; private set; }

    protected virtual void Awake()
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

        saveObj = Instantiate(savePrefab);
        saveObj.name = "SaveManager";
        saveManager = saveObj.GetComponent<SaveManager>();

        StageSelectModel.inStageNum = (StageSelectModel.SelectState)stageNum;
    }

    protected virtual void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        SoundManager.Instance.PlayBGMWithFadeIn("Main", FadeTime);

        blackCurtain.SetActive(false);
        operationUI.SetActive(false);
    }

    void Update()
    {
        stateMachine.UpdateState();
    }

    void FixedUpdate()
    {
        stateMachine.UpdateState();
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

    public void TransitionToSelectScenen()
    {
        SoundManager.Instance.StopBGMWithFadeOut(FadeTime);
        FadeManager.Instance.LoadScene("SelectScene", FadeTime);
    }

    public void TransitionToCurrentScenen()
    {
        SoundManager.Instance.StopBGMWithFadeOut(FadeTime);
        FadeManager.Instance.LoadScene(SceneManager.GetActiveScene().name, FadeTime);
    }
}
