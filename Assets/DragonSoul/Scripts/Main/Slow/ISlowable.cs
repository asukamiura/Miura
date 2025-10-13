public enum SlowTargetType { Player, Enemy }

public interface ISlowable
{
    SlowTargetType Type { get; }
    void ApplySlow(float factor);
    void SetBaseSpeed(float speed);
}
