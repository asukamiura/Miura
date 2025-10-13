using Player;
using System.Collections.Generic;

public class PlayerAttackNormal3 : AttackStateBase<PlayerStateID>
{
    InputReciver Input => InputReciver.Instance;

    public override PlayerStateID StateID => PlayerStateID.AttackNormal3;
    public PlayerAttackNormal3(PlayerCore core) : base(core) { }

    protected override Dictionary<int, AttackAnimationConfig> AnimationData => new()
    {
        {1, new AttackAnimationConfig("AttackNormal3", 0.1f, 0, 0, 0.85f) },
    };

    public override void Update()
    {
        base.Update();

        // ガードステートに遷移
        if (Input.Guard)
        {
            core.stateMachine.ChangeState(PlayerStateID.Guard);
        }

        // ダッシュステートに遷移
        if (Input.Dash)
        {
            core.stateMachine.ChangeState(PlayerStateID.Dash);
        }
    }
}
