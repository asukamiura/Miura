using UnityEngine;
using Player;

public class JustGuardTask : ITutorialTask
{
    private TutorialManager tutorialManager;
    private PlayerStateID previousState;

    private const int needJustGuardCount = 3;               // タスク達成に必要なブロックの回数
    private const int needAttackSpecial2Count = 3;      // タスク達成に必要な特殊攻撃2の回数

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

        if (tutorialManager.Input.Decision)
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
        if (tutorialManager.playerCore.justGuardCount >= needJustGuardCount && tutorialManager.attackSpecial2Count >= needAttackSpecial2Count)
        {
            Debug.Log("Success");
            return true;
        }
        return false;
    }

    private void HandleStateChange(PlayerStateID currentState)
    {
        switch (currentState)
        {           
            case PlayerStateID.AttackSpecial2:
                tutorialManager.attackSpecial2Count++;
                break;
            default:
                break;
        }
    }
}
