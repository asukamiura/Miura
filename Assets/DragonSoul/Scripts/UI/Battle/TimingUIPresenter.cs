using Player;
using UnityEngine;

public class TimingUIPresenter : MonoBehaviour
{
    [SerializeField] TimingUIView view;
    [SerializeField] PlayerCore playerCore;

    void Awake()
    {
        playerCore.OnDodgeTiming += view.ShowTimingUI;
        playerCore.OnGuardTiming += view.ShowTimingUI;
        view.HideTimingUI();
    }

    void OnDisable()
    {
        playerCore.OnDodgeTiming -= view.ShowTimingUI;
        playerCore.OnGuardTiming -= view.ShowTimingUI;
    }
}
