using UnityEngine;
using Player;

public class JustDodgeTask : ITutorialTask
{
    TutorialTaskManager tutorialManager;
    PlayerStateID previousState;

    const int NeedJustDodgeCount = 3;               // タスク達成に必要なブロックの回数
    const int NeedAttackSpecial1Count = 3;      // タスク達成に必要な特殊攻撃2の回数

    public GameObject ExplanationPanel => tutorialManager.justDodgePanel;
    public GameObject TaskUI => tutorialManager.justDodgeTaskUI;
    public bool ShowExplanationPanel => true;
    public bool ShowTaskUI => true;
    public bool ShowCompleteUI => true;

    public JustDodgeTask(TutorialTaskManager tutorialManager)
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
        if (tutorialManager.justDodgeCount >= NeedJustDodgeCount && tutorialManager.attackSpecial1Count >= NeedAttackSpecial1Count)
        {
            return true;
        }
        return false;
    }

    public float TransitionTime() => 2f;

    public float ShowCompleteUITime() => 2.5f;

    void HandleStateChange(PlayerStateID currentState)
    {
        switch (currentState)
        {
            case PlayerStateID.Dodge:
                if (tutorialManager.justDodgeCount == NeedJustDodgeCount) { return; }
                tutorialManager.justDodgeCount++;
                break;
            case PlayerStateID.AttackSpecial1:
                if (tutorialManager.attackSpecial1Count == NeedAttackSpecial1Count) { return; }
                tutorialManager.attackSpecial1Count++;
                break;
            default:
                break;
        }
    }
}
