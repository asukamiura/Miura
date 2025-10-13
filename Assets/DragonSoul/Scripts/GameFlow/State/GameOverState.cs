using UnityEngine;
using System.Collections;

public class GameOverState : IState<GameFlowStateID>
{
    GameFlowManagerBase flowManager;
    GameOverPresenter presenter;

    const float WaitTime = 2;

    public GameOverState(GameFlowManagerBase flowManager, GameOverPresenter presenter)
    {
        this.flowManager = flowManager;
        this.presenter = presenter;
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

        presenter.Open();
        flowManager.ShowBlackCurtain();
        flowManager.ShowOperationUI();
        Time.timeScale = 0;
    }
}
