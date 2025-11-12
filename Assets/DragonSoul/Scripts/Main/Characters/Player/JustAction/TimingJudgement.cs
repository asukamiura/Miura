using UnityEngine;

public class TimingJudgement : MonoBehaviour
{
    [SerializeField] TimingConfig[] dodgeConfig;
    [SerializeField] TimingConfig[] guardConfig;
     
    public TimingType JudgeDodge(float normalizedTime)
    {
        return JudgeTiming(normalizedTime, dodgeConfig);
    }

    public TimingType JudgeGuard(float normalizedTime)
    {
        return JudgeTiming(normalizedTime, guardConfig);
    }

    TimingType JudgeTiming(float normalizedTime, TimingConfig[] timingConfigs)
    {
        foreach (var config in timingConfigs)
        {
            if (config.Contains(normalizedTime))
            {
                return config.timing;
            }
        }
        return TimingType.Miss;
    }
}
