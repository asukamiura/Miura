using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerAttackUltimate : AttackStateBase<PlayerStateID>
    {
        public override PlayerStateID StateID => PlayerStateID.AttackUltimate;

        public PlayerAttackUltimate(PlayerCore core) : base(core) { }

        protected override Dictionary<int, AttackAnimationConfig> AnimationData => new()
        {
            {1, new AttackAnimationConfig("AttackUltimate1", 0.1f, 0, 0, 1, PlayerAttackType.Ultimate) },
            {2, new AttackAnimationConfig("AttackUltimate2", 0, 0, 0, 1, PlayerAttackType.Ultimate) },
            {3, new AttackAnimationConfig("AttackUltimate3", 0, 0, 0, 0.66f, PlayerAttackType.Ultimate) },
        };

        public override void Enter()
        {
            core.IsInvincible = true;
            core.Rb.velocity = Vector3.zero;
            base.Enter();
        }

        public override void Update()
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

                if (step == 3)
                {

                    SlowManager.Instance.ApplySlow(1, SlowTargetType.Enemy);
                }
            }
            else if (core.CurrentStateInfo.normalizedTime >= 1 && core.CurrentStateInfo.IsName(AnimationData[step].animationName))
            {
                core.stateMachine.ChangeState(PlayerStateID.Locomotion);
            }
        }

        public override void FixedUpdate() { }

        public override void Exit()
        {
            base.Exit();
            core.IsInvincible = false;
        }
    }
}
