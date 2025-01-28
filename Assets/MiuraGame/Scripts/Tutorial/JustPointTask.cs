using UnityEngine;

public class JustPointTask : ITutorialTask
{
    TutorialManager tutorialManager;
    PlayerStateID previousState;

    const int needAttackNormalCount = 3;    // タスク達成に必要な通常攻撃の回数

    public GameObject ExplanationPanel => tutorialManager.justPointPanel;
    public GameObject TaskUI => null;
    public bool ShowExplanationPanel => true;
    public bool ShowTaskUI => false;
    public bool ShowSuccessUI => false;

    public JustPointTask(TutorialManager tutorialManager)
    {
        this.tutorialManager = tutorialManager;
    }

    public void Enter()
    {
        tutorialManager.playerCore.enabled = false;
        ExplanationPanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void Update()
    {

    }

    public void Exit()
    {

    }

    public bool CheckTask()
    {
        if (tutorialManager.Input.Decision)
        {
            ExplanationPanel.SetActive(false);
            tutorialManager.playerCore.enabled = true;
            Time.timeScale = 1;
            return true;
        }
        return false;
    }
}
