using UnityEngine;

public interface ITutorialTask
{
    public GameObject ExplanationPanel { get; }     // 説明パネル
    public GameObject TaskUI { get; }               // タスクUI
    public bool ShowExplanationPanel { get; }       // 説明パネルが必要か
    public bool ShowTaskUI { get; }                 // タスクUIが必要か
    public bool ShowCompleteUI { get; }             // 完了UIが必要か
    public void Enter();
    public void Update();
    public void Exit();
    public bool CheckTask();                        // タスクが達成されたかどうか
    public float TransitionTime();                  // 次のタスクに遷移するまでの時間
    public float ShowCompleteUITime();              // 完了UI表示時間
}
