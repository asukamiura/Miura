using UnityEngine.Playables;
using UnityEngine;

public class IntroState : IState<GameFlowStateID>
{
    GameFlowManagerBase flowManager;
    EnemyCoreBase enemyCore;

    public IntroState(GameFlowManagerBase flowManager, EnemyCoreBase enemyCore)
    {
        this.flowManager = flowManager;
        this.enemyCore = enemyCore;
    }

    InputReciver Input => InputReciver.Instance;

    public GameFlowStateID StateID => GameFlowStateID.Intro;

    public void Enter()
    {
        Time.timeScale = 1;

        // プレイヤー操作を無効
        Input.EnablePlayerInput(false);
        TimelineManager.Instance.IntroDirector.stopped += OnTimelineFinished;
        TimelineManager.Instance.IntroDirector.Play();

        flowManager.HideBlackCurtain();
        flowManager.HideOperationUI();

        enemyCore.MoveActive(false);
    }

    public void Update() { }

    public void FixedUpdate() { }

    public void Exit()
    {       
        TimelineManager.Instance.IntroDirector.stopped -= OnTimelineFinished;
    }

    void OnTimelineFinished(PlayableDirector director)
    {
        flowManager.ChangeState(GameFlowStateID.Playing);
    }
}
