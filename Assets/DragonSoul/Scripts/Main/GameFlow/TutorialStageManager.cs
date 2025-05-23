using UnityEngine;

public class TutorialStageManager : GameFlowManager
{
    [SerializeField] PausePanelManager pausePanelManager;
    [SerializeField] GameOverPanelManager gameOverManager;
    [SerializeField] HealthManager playerHealthManager;
    [SerializeField] HealthManager enemyHealthManager;
    [SerializeField] EnemyCoreBase enemyCore;

    public static TutorialStageManager Instance { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

        stateMachine.RegisterState(new IntroState(this, enemyCore));
        stateMachine.RegisterState(new PlayingState(this, playerHealthManager, enemyHealthManager));
        stateMachine.RegisterState(new PauseState(this, pausePanelManager));
        stateMachine.RegisterState(new ClearState(this));
        stateMachine.RegisterState(new GameOverState(this, gameOverManager));
        stateMachine.RegisterState(new TutorialState(this));
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(GameFlowStateID.Intro);
    }
}
