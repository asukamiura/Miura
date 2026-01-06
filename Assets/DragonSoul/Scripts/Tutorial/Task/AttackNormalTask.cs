using UnityEngine;

public class AttackNormalTask : ITutorialTask
{
    TutorialTaskManager tutorialManager;

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
        tutorialManager.playerNormalAttackController.OnAttackExecuted += HandleAttackExecuted;
    }

    public void Update()
    {
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
        tutorialManager.playerNormalAttackController.OnAttackExecuted -= HandleAttackExecuted;
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

    void HandleAttackExecuted(PlayerAttackData attackData)
    {
        if (!attackData.IsFinalStep) { return; }

        tutorialManager.attackNormalCount++;
    }
}
