using UnityEngine;

public class PlayingState : IState<GameFlowStateID>
{
    GameFlowManagerBase flowManager;
    EnemyCoreBase enemyCore;
    bool ignorePause = true;

    public PlayingState(GameFlowManagerBase flowManager, EnemyCoreBase enemyCore)
    {
        this.flowManager = flowManager;
        this.enemyCore = enemyCore;
    }

    InputReciver Input => InputReciver.Instance;

    public GameFlowStateID StateID => GameFlowStateID.Playing;

    public void Enter()
    {
        Time.timeScale = flowManager.PreviousTimeScale;

        Input.EnablePlayerInput(true);

        enemyCore.MoveActive(true);
    }

    public void Update() 
    {
        if (ignorePause)
        {
            // Pauseボタンが押されていないのを確認するまで待つ
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
    }

    public void FixedUpdate() { }

    public void Exit() 
    {
        enemyCore.MoveActive(false);

        // プレイヤー操作無効
        Input.EnablePlayerInput(false);

        ignorePause = true;
    }
}
