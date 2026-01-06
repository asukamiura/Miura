using UnityEngine;

public class TutorialState : IState<GameFlowStateID>
{
    GameFlowManagerBase flowManager;

    public TutorialState(GameFlowManagerBase flowManager)
    {
        this.flowManager = flowManager;
    }

    InputReciver Input => InputReciver.Instance;

    public GameFlowStateID StateID => GameFlowStateID.Tutorial;

    public void Enter()
    {
        // チュートリアル操作を有効
        Input.EnableTutorialInput(true);

        // UI操作を有効
        Input.EnableUIInput(true);

        flowManager.PreviousTimeScale = Time.timeScale;
        Time.timeScale = 0;
    }

    public void Update()
    {
        if (Input.Pause)
        {
            flowManager.ChangeState(GameFlowStateID.Pause);
        }
    }

    public void FixedUpdate() { }

    public void Exit()
    {      
        Time.timeScale = flowManager.PreviousTimeScale;

        // チュートリアル操作無効
        Input.EnableTutorialInput(false);

        // UI操作を無効
        Input.EnableUIInput(false);
    }
}
