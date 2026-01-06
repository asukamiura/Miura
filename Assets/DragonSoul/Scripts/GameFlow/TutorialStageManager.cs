public class TutorialStageManager : GameFlowManagerBase
{
    protected override void Awake()
    {
        base.Awake();

        playerHealthManager.OnDied += () => ChangeState(GameFlowStateID.GameOver);
        enemyHealthManager.OnDied += () => ChangeState(GameFlowStateID.Clear);

        stateMachine.RegisterState(new IntroState(this, enemyCore));
        stateMachine.RegisterState(new PlayingState(this, enemyCore));
        stateMachine.RegisterState(new PauseState(this, pauseMenuPresenter));
        stateMachine.RegisterState(new ClearState(this));
        stateMachine.RegisterState(new GameOverState(this, gameOverPresenter));
        stateMachine.RegisterState(new TutorialState(this));
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(GameFlowStateID.Intro);
    }
}
