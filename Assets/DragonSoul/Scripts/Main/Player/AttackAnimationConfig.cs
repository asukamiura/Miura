using Player;

public class AttackAnimationConfig
{
    public string animationName;
    public float transitionDuration;
    public int layer = 0;
    public float offset = 0;
    public float nextStateTransitionTime = 1;
    public AttackType attackType = AttackType.None;
}
