using Player;
using System.Collections.Generic;

public class PlayerAttackNormal3 : AttackStateBase<PlayerStateID>
{
    InputReciver Input => InputReciver.Instance;

    public override PlayerStateID StateID => PlayerStateID.AttackNormal3;
    public PlayerAttackNormal3(PlayerCore core, PlayerAttack playerAttack, AttackAssist attackAssist) : base(core, playerAttack, attackAssist) { }

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
            core.StateMachine.ChangeState(PlayerStateID.Guard);
        }

        // ダッシュステートに遷移
        if (Input.Dash)
        {
            core.StateMachine.ChangeState(PlayerStateID.Dash);
        }
    }
}
