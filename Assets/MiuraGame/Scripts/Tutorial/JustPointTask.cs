using UnityEngine;

public class JustPointTask : ITutorialTask
{
    TutorialManager tutorialManager;
    PlayerStateID previousState;

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
        if (tutorialManager.Input.Decision && ExplanationPanel.activeSelf)
        {
            ExplanationPanel.SetActive(false);
            tutorialManager.playerCore.enabled = true;
            Time.timeScale = 1;
            return true;
        }
        return false;
    }

    public float TransitionTime() => 2f;
}
