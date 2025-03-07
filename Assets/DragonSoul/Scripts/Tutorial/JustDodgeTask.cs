using UnityEngine;

public class JustDodgeTask : ITutorialTask
{
    TutorialManager tutorialManager;
    PlayerStateID previousState;

    const int NeedJustDodgeCount = 3;               // タスク達成に必要なブロックの回数
    const int NeedAttackSpecial1Count = 3;      // タスク達成に必要な特殊攻撃2の回数

    public GameObject ExplanationPanel => tutorialManager.justDodgePanel;
    public GameObject TaskUI => tutorialManager.justDodgeTaskUI;
    public bool ShowExplanationPanel => true;
    public bool ShowTaskUI => true;
    public bool ShowSuccessUI => true;

    public JustDodgeTask(TutorialManager tutorialManager)
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
            Debug.Log("完了");
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

    public float TransitionTime() => 3f;

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
