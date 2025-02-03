using UnityEngine;

public class JustGuardTask : ITutorialTask
{
    TutorialManager tutorialManager;
    PlayerStateID previousState;

    const int NeedJustGuardCount = 3;               // タスク達成に必要なブロックの回数
    const int NeedAttackSpecial2Count = 3;      // タスク達成に必要な特殊攻撃2の回数

    public GameObject ExplanationPanel => tutorialManager.justGuardPanel;
    public GameObject TaskUI => tutorialManager.justGuardTaskUI;
    public bool ShowExplanationPanel => true;
    public bool ShowTaskUI => true;
    public bool ShowSuccessUI => true;

    public JustGuardTask(TutorialManager tutorialManager)
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
        if (tutorialManager.justGuardCount >= NeedJustGuardCount && tutorialManager.attackSpecial2Count >= NeedAttackSpecial2Count)
        {
            Debug.Log("Success");
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
