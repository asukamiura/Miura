using Player;
using UnityEngine;

public class TimingUIPresenter : MonoBehaviour
{
    [SerializeField] TimingUIView view;

    void Awake()
    {
        PlayerDash.OnJudgeDodgeTiming += view.ShowTimingUI;
        PlayerGuard.OnJudgeGuardTiming += view.ShowTimingUI;        
        view.HideTimingUI();
    }

    void OnDisable()
    {
        PlayerDash.OnJudgeDodgeTiming -= view.ShowTimingUI;
        PlayerGuard.OnJudgeGuardTiming -= view.ShowTimingUI;
    }
}
