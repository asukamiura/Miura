using UnityEngine;
using System.Collections;
using SoundSystem;

public class ClearState : IState<GameFlowStateID>
{
    GameFlowManager flowManager;

    public ClearState(GameFlowManager flowManager)
    {
        this.flowManager = flowManager;
    }

    InputReciver Input => InputReciver.Instance;

    public GameFlowStateID StateID => GameFlowStateID.Clear;

    public void Enter() 
    {
        TimelineManager.Instance.ClearDirector.Play();
        Time.timeScale = 0;
        PostEffectManager.Instance.ChangePostEffect(ProfileNum.Clear, 0);

        flowManager.StartCoroutine(ChangeScene(2.5f));
    }

    public void Update() { }

    public void FixedUpdate() { }

    public void Exit() { }

    IEnumerator ChangeScene(float delay)
    {
        yield return new WaitForSecondsRealtime(6.5f);

        Time.timeScale = 1;

        yield return new WaitForSecondsRealtime(delay);

        SoundManager.Instance.StopBGMWithFadeOut("Main");
        FadeManager.Instance.LoadScene("ResultScene");
    }
}
