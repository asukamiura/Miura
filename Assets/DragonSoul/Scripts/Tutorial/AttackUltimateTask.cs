using UnityEngine;
using Player;

public class AttackUltimateTask : ITutorialTask
{
    TutorialTaskManager tutorialManager;
    PlayerStateID previousState;

    const int NeedAttackUltimateCount = 1;    // タスク達成に必要な通常攻撃の回数

    public GameObject ExplanationPanel => tutorialManager.attackUltimatePanel;
    public GameObject TaskUI => tutorialManager.attackUltimateTaskUI;
    public bool ShowExplanationPanel => true;
    public bool ShowTaskUI => true;
    public bool ShowCompleteUI => true;

    public AttackUltimateTask(TutorialTaskManager tutorialManager)
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

            GameFlowManagerBase.Instance.ChangeState(GameFlowStateID.Playing);
        }
    }

    public void Exit()
    {
        TaskUI.SetActive(false);
    }

    public bool CheckTask()
    {
        if (tutorialManager.attackUltimateCount >= NeedAttackUltimateCount)
        {
            return true;
        }
        return false;
    }

    public float TransitionTime() => 2f;

    public float ShowCompleteUITime() => 3.5f;

    void HandleStateChange(PlayerStateID currentState)
    {
        switch (currentState)
        {
            case PlayerStateID.AttackUltimate:
                tutorialManager.attackUltimateCount++;
                break;
            default:
                break;
        }
    }
}
