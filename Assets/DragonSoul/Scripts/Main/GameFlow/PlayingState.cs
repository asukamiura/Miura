using UnityEngine;

public class PlayingState : IState<GameFlowStateID>
{
    GameFlowManagerBase flowManager;
    HealthManager playerHealthManager;
    HealthManager enemyHealthManager;
    bool ignorePause = true;

    public PlayingState(GameFlowManagerBase flowManager, HealthManager playerHealthManager, HealthManager enemyHealthManager)
    {
        this.flowManager = flowManager;
        this.playerHealthManager = playerHealthManager;
        this.enemyHealthManager = enemyHealthManager;
    }

    InputReciver Input => InputReciver.Instance;

    public GameFlowStateID StateID => GameFlowStateID.Playing;

    public void Enter()
    {
        Time.timeScale = flowManager.PreviousTimeScale;

        Input.EnablePlayerInput(true);
    }

    public void Update() 
    {
        if (ignorePause)
        {
            if (!Input.Pause)
            {
                ignorePause = false;
            }
        }
        else 
        {
            if (Input.Pause)
            {
                flowManager.ChangeState(GameFlowStateID.Pause);
            }
        }

        if (playerHealthManager.IsDead)
        {
            flowManager.ChangeState(GameFlowStateID.GameOver);
        }

        if (enemyHealthManager.IsDead)
        {
            flowManager.ChangeState(GameFlowStateID.Clear);
        }
    }

    public void FixedUpdate() { }

    public void Exit() 
    {
        // プレイヤー操作無効
        Input.EnablePlayerInput(false);
        ignorePause = true;
    }
}
