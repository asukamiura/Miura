using UnityEngine;

public interface ITutorialTask
{
    GameObject ExplanationPanel { get; }
    GameObject TaskUI { get; }
    bool ShowExplanationPanel { get; }
    bool ShowTaskUI { get; }
    bool ShowCompleteUI { get; }
    public void Enter();
    public void Update();
    public void Exit();
    public bool CheckTask();
    public float TransitionTime();
    public float ShowCompleteUITime();
}
