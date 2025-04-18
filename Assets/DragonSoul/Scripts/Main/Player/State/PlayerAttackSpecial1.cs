using System.Collections.Generic;

namespace Player
{
    public class PlayerAttackSpecial1 : AttackStateBase<PlayerStateID>
    {
        public override PlayerStateID StateID => PlayerStateID.AttackSpecial1;

        protected override Dictionary<int, AttackAnimationConfig> AnimationData => new()
        {
            {1, new AttackAnimationConfig{ animationName = "AttackSpecial1_1", transitionDuration = 0.1f, layer = 0, offset = 0.3f, nextStateTransitionTime = 0.7f, attackType = AttackType.Special1_1} },
            {2, new AttackAnimationConfig{ animationName = "AttackSpecial1_2", transitionDuration = 0.1f, layer = 0, offset = 0.3f, nextStateTransitionTime = 0.7f,  attackType = AttackType.Special1_2} },
            {3, new AttackAnimationConfig{ animationName = "AttackSpecial1_3", transitionDuration = 0.1f, layer = 0, offset = 0.1f, nextStateTransitionTime = 0.8f,  attackType = AttackType.Special1_3} },            
        };

        public PlayerAttackSpecial1(PlayerCore core) : base(core) { }

        public override void Enter()
        {
            base.Enter();
            core.Animator.applyRootMotion = true;
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
