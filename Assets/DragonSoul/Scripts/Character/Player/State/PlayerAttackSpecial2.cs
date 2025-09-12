using System.Collections.Generic;

namespace Player
{
    public class PlayerAttackSpecial2 : AttackStateBase<PlayerStateID>
    {
        public override PlayerStateID StateID => PlayerStateID.AttackSpecial2;
        InputReciver Input => InputReciver.Instance;

        protected override Dictionary<int, AttackAnimationConfig> AnimationData => new()
        {
            {1, new AttackAnimationConfig("AttackSpecial2_1", 0.1f, 0, 0.15f, 0.645f, PlayerAttackType.Special2_1) },
            {2, new AttackAnimationConfig("AttackSpecial2_2", 0, 0, 0.5f, 1, PlayerAttackType.Special2_2) },
            {3, new AttackAnimationConfig("AttackSpecial2_3", 0, 0, 0, 1, PlayerAttackType.Special2_3) },
            {4, new AttackAnimationConfig("AttackSpecial2_4", 0, 0, 0, 1, PlayerAttackType.Special2_4) }
        };

        public PlayerAttackSpecial2(PlayerCore core) : base(core) { }

        public override void Enter()
        {
            base.Enter();
            core.IsInvincible = true;            
        }

        public override void Update()
        { 
            base.Update();
        }

        public override void FixedUpdate() { }

        public override void Exit()
        {
            base.Exit();
            core.Animator.speed = 1;
            core.IsInvincible = false;
        }
    }
}
