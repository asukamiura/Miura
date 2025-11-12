using UnityEngine;
using Player;

public class JustGuardTask : ITutorialTask
{
    TutorialTaskManager tutorialManager;
    PlayerStateID previousState;

    const int NeedJustGuardCount = 3;               // タスク達成に必要なブロックの回数
    const int NeedAttackSpecial2Count = 3;      // タスク達成に必要な特殊攻撃2の回数

    public GameObject ExplanationPanel => tutorialManager.justGuardPanel;
    public GameObject TaskUI => tutorialManager.justGuardTaskUI;
    public bool ShowExplanationPanel => true;
    public bool ShowTaskUI => true;
    public bool ShowCompleteUI => true;

    public JustGuardTask(TutorialTaskManager tutorialManager)
    {
        this.tutorialManager = tutorialManager;
    }

    public void Enter()
    {        
        ExplanationPanel.SetActive(true);
        
        previousState = tutorialManager.playerCore.StateMachine.CurrentState;
    }

    public void Update()
    {
        // プレイヤーの状態が変化したときのみ処理
        if (tutorialManager.playerCore.StateMachine.CurrentState != previousState)
        {
            HandleStateChange(tutorialManager.playerCore.StateMachine.CurrentState);
            previousState = tutorialManager.playerCore.StateMachine.CurrentState;
        }

        if (tutorialManager.Input.GoNext && ExplanationPanel.activeSelf)
        {          
            ExplanationPanel.SetActive(false);
 
            TaskUI.SetActive(true);

            GameFlowManagerBase.Instance.ChangeState(GameFlowStateID.Playing);
        }
    }

    public void Exit()
    {
        TaskUI.SetActive(false);
    }

    public bool CheckTask()
    {
        if (tutorialManager.justGuardCount >= NeedJustGuardCount && tutorialManager.attackSpecial2Count >= NeedAttackSpecial2Count)
        {
            return true;
        }
        return false;
    }

    public float TransitionTime() => 2f;

    public float ShowCompleteUITime() => 4f;

    void HandleStateChange(PlayerStateID currentState)
    {
        switch (currentState)
        {
            case PlayerStateID.Block:
                if (tutorialManager.justGuardCount == NeedJustGuardCount) { return; }
                tutorialManager.justGuardCount++;
                break;
            case PlayerStateID.AttackSpecial2:
                if (tutorialManager.attackSpecial2Count == NeedAttackSpecial2Count) { return; }
                tutorialManager.attackSpecial2Count++;
                break;
            default:
                break;
        }
    }
}
