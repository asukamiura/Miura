using UnityEngine;
using Player;

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
        ExplanationPanel.SetActive(true);
    }

    public void Update()
    {

    }

    public void Exit()
    {

    }

    public bool CheckTask()
    {
        if (tutorialManager.Input.GoNext && ExplanationPanel.activeSelf)
        {
            ExplanationPanel.SetActive(false);

            TutorialStageManager.Instance.ChangeState(GameFlowStateID.Playing);

            return true;
        }
        return false;
    }

    public float TransitionTime() => 2f;
}
