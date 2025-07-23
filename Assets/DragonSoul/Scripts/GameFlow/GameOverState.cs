using UnityEngine;
using System.Collections;

public class GameOverState : IState<GameFlowStateID>
{
    GameFlowManagerBase flowManager;
    GameOverPanelManager panelManager;

    const float WaitTime = 2;

    public GameOverState(GameFlowManagerBase flowManager, GameOverPanelManager panelManager)
    {
        this.flowManager = flowManager;
        this.panelManager = panelManager;
    }

    InputReciver Input => InputReciver.Instance;

    public GameFlowStateID StateID => GameFlowStateID.GameOver;

    public void Enter()
    {
        flowManager.StartCoroutine(ShowGameOverPanel());

        Input.EnableUIInput(true);
    }

    public void Update() { }

    public void FixedUpdate() { }

    public void Exit()
    {

    }

    IEnumerator ShowGameOverPanel()
    {
        yield return new WaitForSeconds(WaitTime);

        panelManager.ShowGameOverPanel();
        flowManager.ShowBlackCurtain();
        flowManager.ShowOperationUI();
        Time.timeScale = 0;
    }
}
