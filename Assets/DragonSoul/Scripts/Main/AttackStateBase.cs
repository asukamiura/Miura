using Player;
using System.Collections.Generic;

public abstract class AttackStateBase<TStateID> : IState<TStateID>
{
    public AttackStateBase(PlayerCore core)
    {
        this.core = core;
    }
    
    public virtual TStateID StateID => default;

    protected int step = 1;

    protected int MaxStep => AnimationData.Count;

    protected PlayerCore core;

    protected abstract Dictionary<int, AttackAnimationConfig> AnimationData { get; }

    public virtual void Enter() 
    {
        core.Animator.CrossFade(AnimationData[step].animationName, AnimationData[step].transitionDuration, AnimationData[step].layer, AnimationData[step].offset);

        core.powerManager.SetAttackType(AnimationData[step].attackType);
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
                core.Animator.CrossFade(AnimationData[step].animationName, AnimationData[step].transitionDuration, AnimationData[step].layer, AnimationData[step].offset);

                core.powerManager.SetAttackType(AnimationData[step].attackType);
            }
        }
    }

    public virtual void FixedUpdate() { }

    public virtual void Exit() 
    {
        step = 1;
        core.attackAssist.StopAssist();
    }
}
