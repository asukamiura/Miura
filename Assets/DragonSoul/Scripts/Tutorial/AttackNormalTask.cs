using UnityEngine;
using Player;

public class AttackNormalTask : ITutorialTask
{
    TutorialManager tutorialManager;
    PlayerStateID previousState;

    const int NeedAttackNormalCount = 3;    // タスク達成に必要な通常攻撃の回数

    public GameObject ExplanationPanel => tutorialManager.attackNormalPanel;
    public GameObject TaskUI => tutorialManager.attackNormalTaskUI;
    public bool ShowExplanationPanel => true;
    public bool ShowTaskUI => true;
    public bool ShowSuccessUI => true;

    public AttackNormalTask(TutorialManager tutorialManager)
    {
        this.tutorialManager = tutorialManager;
    }

    public void Enter()
    {
        tutorialManager.playerCore.enabled = false;
        ExplanationPanel.SetActive(true);
        Time.timeScale = 0;
        previousState = tutorialManager.playerCore.stateMachine.StateID;
    }

    public void Update()
    {
        // プレイヤーの状態が変化したときのみ処理
        if (tutorialManager.playerCore.stateMachine.StateID != previousState)
        {
            HandleStateChange(tutorialManager.playerCore.stateMachine.StateID);
            previousState = tutorialManager.playerCore.stateMachine.StateID;
        }

        if (tutorialManager.Input.Decision && ExplanationPanel.activeSelf)
        {
            ExplanationPanel.SetActive(false);
            Time.timeScale = 1;
            tutorialManager.playerCore.enabled = true;
            TaskUI.SetActive(true);
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

    void HandleStateChange(PlayerStateID currentState)
    {
        switch (currentState)
        {
            case PlayerStateID.AttackNormal:
                tutorialManager.attackNormalCount++;
                break;
            default:
                break;
        }
    }
}
