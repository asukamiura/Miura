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
    public bool ShowSuccessUI => true;

    public JustGuardTask(TutorialTaskManager tutorialManager)
    {
        this.tutorialManager = tutorialManager;
    }

    public void Enter()
    {        
        ExplanationPanel.SetActive(true);
        
        previousState = tutorialManager.playerCore.stateMachine.CurrentState;
    }

    public void Update()
    {
        // プレイヤーの状態が変化したときのみ処理
        if (tutorialManager.playerCore.stateMachine.CurrentState != previousState)
        {
            HandleStateChange(tutorialManager.playerCore.stateMachine.CurrentState);
            previousState = tutorialManager.playerCore.stateMachine.CurrentState;
        }

        if (tutorialManager.Input.GoNext && ExplanationPanel.activeSelf)
        {          
            ExplanationPanel.SetActive(false);
 
            TaskUI.SetActive(true);

            TutorialStageManager.Instance.ChangeState(GameFlowStateID.Playing);
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

    public float TransitionTime() => 5f;

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
