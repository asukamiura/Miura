using UnityEngine;

public class TutorialStageManager : GameFlowManagerBase
{
    [SerializeField] PauseMenuPresenter pauseMenuPresenter;
    [SerializeField] GameOverPresenter gameOverPresenter;

    protected override void Awake()
    {
        base.Awake();

        stateMachine.RegisterState(new IntroState(this, enemyCore));
        stateMachine.RegisterState(new PlayingState(this, playerHealthManager, enemyHealthManager, enemyCore));
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
