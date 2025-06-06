using UnityEngine;
using System.Collections;
using SoundSystem;
using UnityEngine.Playables;

public class ClearState : IState<GameFlowStateID>
{
    GameFlowManagerBase flowManager;
    const float ChangeTiming = 2f;

    public ClearState(GameFlowManagerBase flowManager)
    {
        this.flowManager = flowManager;
    }

    InputReciver Input => InputReciver.Instance;

    public GameFlowStateID StateID => GameFlowStateID.Clear;

    public void Enter() 
    {
        TimelineManager.Instance.ClearDirector.Play();

        PostEffectManager.Instance.ChangePostEffect(ProfileNum.Clear, 0);

        flowManager.StartCoroutine(ChangeTimeScale(ChangeTiming));
        flowManager.StartCoroutine(WaitForTimelineEnd());
    }

    public void Update() { }

    public void FixedUpdate() { }

    public void Exit() { }

    IEnumerator ChangeTimeScale(float delay)
    {
        Time.timeScale = 0;

        yield return new WaitForSecondsRealtime(delay);

        Time.timeScale = 1;
    }

    void ChangeScene(PlayableDirector director)
    {
        SoundManager.Instance.StopBGMWithFadeOut("Main");
        FadeManager.Instance.LoadScene("ResultScene");
    }

    IEnumerator WaitForTimelineEnd()
    {
        var director = TimelineManager.Instance.ClearDirector;
        while (director.time < director.duration)
        {
            yield return null;
        }

        ChangeScene(director);
    }
}
