using UnityEngine;
using Player;

public class JustPointTask : ITutorialTask
{
    TutorialTaskManager tutorialManager;
    PlayerStateID previousState;

    public GameObject ExplanationPanel => tutorialManager.justPointPanel;
    public GameObject TaskUI => null;
    public bool ShowExplanationPanel => true;
    public bool ShowTaskUI => false;
    public bool ShowCompleteUI => false;

    public JustPointTask(TutorialTaskManager tutorialManager)
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

            GameFlowManagerBase.Instance.ChangeState(GameFlowStateID.Playing);

            return true;
        }
        return false;
    }

    public float TransitionTime() => 2f;

    public float ShowCompleteUITime() => 0;
}
