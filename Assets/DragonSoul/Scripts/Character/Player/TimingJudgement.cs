using UnityEngine;

public class TimingJudgement : MonoBehaviour
{
    [SerializeField] TimingConfig[] dodgeConfig;
    [SerializeField] TimingConfig[] guardConfig;

    public Timing JudgeDodge(float normalizedTime)
    {
        return JudgeTiming(normalizedTime, dodgeConfig);
    }

    public Timing JudgeGuard(float normalizedTime)
    {
        return JudgeTiming(normalizedTime, guardConfig);
    }

    Timing JudgeTiming(float normalizedTime, TimingConfig[] timingConfigs)
    {
        Timing result = Timing.None;
        foreach (var config in timingConfigs)
        {
            if (config.Contains(normalizedTime))
            {
                result = config.timing;
            }
        }
        return result;
    }
}
