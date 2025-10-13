public enum Timing { Miss, Fast, Just, Late }

[System.Serializable]
public struct TimingConfig
{
    public Timing timing;
    public float start;
    public float end;

    public bool Contains(float normalizedTime)
    {
        return normalizedTime >= start && normalizedTime < end;
    }
}