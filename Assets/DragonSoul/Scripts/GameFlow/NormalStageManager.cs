using UnityEngine;

public class NormalStageManager : GameFlowManagerBase
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
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(GameFlowStateID.Intro);
    }
}
