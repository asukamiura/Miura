using Player;
using System.Collections.Generic;

public abstract class AttackStateBase<TStateID> : IState<TStateID>
{    
    protected int step = 1;
    protected PlayerCore core;
    protected abstract Dictionary<int, AttackAnimationConfig> AnimationData { get; }
    protected int MaxStep => AnimationData.Count;

    public virtual TStateID StateID => default;

    public AttackStateBase(PlayerCore core)
    {
        this.core = core;
    }

    public virtual void Enter() 
    {
        PlayCurrentAnimation();
        core.Animator.applyRootMotion = true;
    }

    public virtual void Update() 
    {
        if (core.CurrentStateInfo.normalizedTime >= AnimationData[step].nextStateTransitionTime && core.CurrentStateInfo.IsName(AnimationData[step].animationName))
        {
            step++;

            if (step > MaxStep)
            {
                core.stateMachine.ChangeState(PlayerStateID.Locomotion);
            }
            else
            {
                PlayCurrentAnimation();
            }
        }
    }

    public virtual void FixedUpdate() { }

    public virtual void Exit() 
    {
        step = 1;
        core.attackAssist.StopAssist();
        core.Animator.applyRootMotion = false;
    }

    // ステップに登録されたアニメーションを再生
    protected void PlayCurrentAnimation()
    {
        var config = AnimationData[step];

        core.Animator.CrossFade(config.animationName, config.transitionDuration, config.layer, config.offset);

        core.attackTypeHolder.SetAttackType(config.attackType);
    }

    protected class AttackAnimationConfig
    {
        public readonly string animationName;
        public readonly float transitionDuration;
        public readonly int layer = 0;
        public readonly float offset = 0;
        public readonly float nextStateTransitionTime = 1;
        public readonly AttackType attackType = AttackType.None;

        public AttackAnimationConfig(string animationName, float transitionDuration, int layer, float offset, float nextStateTransitionTime, AttackType attackType)
        {
            this.animationName = animationName;
            this.transitionDuration = transitionDuration;
            this.layer = layer;
            this.offset = offset;
            this.nextStateTransitionTime = nextStateTransitionTime;
            this.attackType = attackType;
        }
    }
}
