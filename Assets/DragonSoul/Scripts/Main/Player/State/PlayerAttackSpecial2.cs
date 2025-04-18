using System.Collections.Generic;

namespace Player
{
    public class PlayerAttackSpecial2 : AttackStateBase<PlayerStateID>
    {
        public override PlayerStateID StateID => PlayerStateID.AttackSpecial2;
        InputReciver Input => InputReciver.Instance;

        protected override Dictionary<int, AttackAnimationConfig> AnimationData => new()
        {
            {1, new AttackAnimationConfig{ animationName = "AttackSpecial2_1", transitionDuration = 0.1f, layer = 0, offset = 0.1f, nextStateTransitionTime = 0.645f, attackType = AttackType.Special2_1} },
            {2, new AttackAnimationConfig{ animationName = "AttackSpecial2_2", transitionDuration = 0, layer = 0, offset = 0.5f, nextStateTransitionTime = 1,  attackType = AttackType.Special2_2} },
            {3, new AttackAnimationConfig{ animationName = "AttackSpecial2_3", transitionDuration = 0, layer = 0, offset = 0, nextStateTransitionTime = 1,  attackType = AttackType.Special2_3} },
            {4, new AttackAnimationConfig{ animationName = "AttackSpecial2_4", transitionDuration = 0, layer = 0, offset = 0, nextStateTransitionTime = 1, attackType = AttackType.Special2_4} }
        };

        public PlayerAttackSpecial2(PlayerCore core) : base(core) { }

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
