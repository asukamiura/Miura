using UnityEngine;

public class PauseState : IState<GameFlowStateID>
{
    GameFlowManager flowManager;
    PausePanelManager panelManager;

    GameFlowStateID transitionStateID;

    public PauseState(GameFlowManager flowManager, PausePanelManager panelManager)
    {
        this.flowManager = flowManager;
        this.panelManager = panelManager;          
    }

    InputReciver Input => InputReciver.Instance;

    public GameFlowStateID StateID => GameFlowStateID.Pause;



    public void Enter() 
    {
        transitionStateID = flowManager.PreivousState;
        panelManager.ClosePressed += () => flowManager.ChangeState(transitionStateID);

        flowManager.ShowBlackCurtain();
        flowManager.ShowOperationUI();

        // UI操作有効
        Input.EnableUIInput(true);

        flowManager.PreviousTimeScale = Time.timeScale;
        Time.timeScale = 0;

        // ポーズ画面を表示
        panelManager.ShowPausePanel();
    }
    
    public void Update() 
    {
       
    }
    
    public void FixedUpdate() { }
    
    public void Exit() 
    {
        // ポーズ画面を非表示
        panelManager.HidePausePanel();

        flowManager.HideBlackCurtain();
        flowManager.HideOperationUI();

        Time.timeScale = flowManager.PreviousTimeScale;

        // UI操作を無効
        Input.EnableUIInput(false);

        panelManager.ClosePressed -= () => flowManager.ChangeState(transitionStateID);
    }
}
