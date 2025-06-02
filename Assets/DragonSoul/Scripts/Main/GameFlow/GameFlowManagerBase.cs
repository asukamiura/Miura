using SoundSystem;
using UnityEngine;

public class GameFlowManagerBase : MonoBehaviour
{
    [SerializeField] GameObject blackCurtain;
    [SerializeField] GameObject operationUI;
    [SerializeField] GameObject savePrefab;

    protected StateMachine<GameFlowStateID> stateMachine;
    GameObject saveObj;
    SaveManager saveManager;

    const float FadeTime = 1;

    public GameFlowStateID CurrentState => stateMachine.CurrentState;
    public GameFlowStateID PreivousState => stateMachine.PreviousState;
    public float PreviousTimeScale { get; set; } = 1;

    protected virtual void Awake()
    {       
        stateMachine = new StateMachine<GameFlowStateID>();

        saveObj = Instantiate(savePrefab);
        saveObj.name = "SaveManager";
        saveManager = saveObj.GetComponent<SaveManager>();
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
        stateMachine.StateUpdate();       
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
