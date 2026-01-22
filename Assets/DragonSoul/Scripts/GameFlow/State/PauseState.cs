using UnityEngine;

public class PauseState : IState<GameFlowStateID>
{
    GameFlowManagerBase flowManager;
    PauseMenuPresenter presenter;
    //PausePanelManager panelManager;

    GameFlowStateID transitionStateID;

    public PauseState(GameFlowManagerBase flowManager, PauseMenuPresenter presenter)
    {
        this.flowManager = flowManager;
        this.presenter = presenter;          
    }

    InputReceiver Input => InputReceiver.Instance;

    public GameFlowStateID StateID => GameFlowStateID.Pause; 

    public void Enter() 
    {
        transitionStateID = flowManager.PreivousState;
        //panelManager.ClosePressed += () => flowManager.ChangeState(transitionStateID);

        flowManager.ShowBlackCurtain();
        flowManager.ShowOperationUI();

        // UI操作有効
        Input.EnableUIInput(true);

        flowManager.PreviousTimeScale = Time.timeScale;
        Time.timeScale = 0;

        // ポーズ画面を表示
        //panelManager.ShowPausePanel();
        presenter.Open(onClose: () => flowManager.ChangeState(transitionStateID));
    }
    
    public void Update() 
    {
       
    }
    
    public void FixedUpdate() { }
    
    public void Exit() 
    {
        // ポーズ画面を非表示
        //panelManager.HidePausePanel();
        presenter.Close();

        flowManager.HideBlackCurtain();
        flowManager.HideOperationUI();

        Time.timeScale = flowManager.PreviousTimeScale;

        // UI操作を無効
        Input.EnableUIInput(false);

        //panelManager.ClosePressed -= () => flowManager.ChangeState(transitionStateID);
    }
}
