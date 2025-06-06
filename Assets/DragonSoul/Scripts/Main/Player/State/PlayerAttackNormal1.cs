using Player;
using System.Collections.Generic;

public class PlayerAttackNormal1 : AttackStateBase<PlayerStateID>
{
    InputReciver Input => InputReciver.Instance;

    public override PlayerStateID StateID => PlayerStateID.AttackNormal1;
    public PlayerAttackNormal1(PlayerCore core) : base(core) { }

    protected override Dictionary<int, AttackAnimationConfig> AnimationData => new()
    {
        {1, new AttackAnimationConfig("AttackNormal1", 0.1f, 0, 0, 0.6f, AttackType.Normal1) },
    };

    bool isCombo = false;

    public override void Update()
    {
        if (Input.AttackNormal)
        {
            isCombo = true;
        }

        if (core.CurrentStateInfo.IsName(AnimationData[step].animationName))
        {
            if (isCombo && core.CurrentStateInfo.normalizedTime >= AnimationData[step].nextStateTransitionTime)
            {
                core.stateMachine.ChangeState(PlayerStateID.AttackNormal2);
            }
            else if (!isCombo && core.CurrentStateInfo.normalizedTime >= 1.0)
            {
                core.stateMachine.ChangeState(PlayerStateID.Locomotion);
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
        isCombo = false;
    }
}
