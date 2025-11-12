using UnityEngine;
using Player;

public class AttackNormalTask : ITutorialTask
{
    TutorialTaskManager tutorialManager;
    PlayerStateID previousState;

    const int NeedAttackNormalCount = 3;    // タスク達成に必要な通常攻撃の回数

    public GameObject ExplanationPanel => tutorialManager.attackNormalPanel;
    public GameObject TaskUI => tutorialManager.attackNormalTaskUI;
    public bool ShowExplanationPanel => true;
    public bool ShowTaskUI => true;
    public bool ShowCompleteUI => true;

    public AttackNormalTask(TutorialTaskManager tutorialManager)
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
        if (tutorialManager.attackNormalCount >= NeedAttackNormalCount)
        {
            return true;
        }
        return false;
    }

    public float TransitionTime() => 2f;

    public float ShowCompleteUITime() => 1f;

    void HandleStateChange(PlayerStateID currentState)
    {
        switch (currentState)
        {
            case PlayerStateID.AttackNormal3:
                tutorialManager.attackNormalCount++;
                break;
            default:
                break;
        }
    }
}
