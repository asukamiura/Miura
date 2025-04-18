using System.Collections.Generic;

namespace Player
{
    public class PlayerAttackNormal : AttackStateBase<PlayerStateID>
    {
        public PlayerAttackNormal(PlayerCore core) : base(core) { }

        public override PlayerStateID StateID => PlayerStateID.AttackNormal;

        protected override Dictionary<int, AttackAnimationConfig> AnimationData => new()
        {
            {1, new AttackAnimationConfig{ animationName = "AttackNormal1", transitionDuration = 0.1f, layer = 0, offset = 0, nextStateTransitionTime = 0.6f, attackType = AttackType.Normal1} },
            {2, new AttackAnimationConfig{ animationName = "AttackNormal2", transitionDuration = 0.1f, layer = 0, offset = 0, nextStateTransitionTime = 0.6f,  attackType = AttackType.Normal2} },
            {3, new AttackAnimationConfig{ animationName = "AttackNormal3", transitionDuration = 0.1f, layer = 0, offset = 0, nextStateTransitionTime = 0.85f,  attackType = AttackType.Normal3} },
        };

        InputReciver Input => InputReciver.Instance;

        bool canContinueCombo = false;

        public override void Enter()
        {
            base.Enter();
        }

        public override void Update()
        {
            if (Input.AttackNormal)
            {
                canContinueCombo = true;
            }

            if (canContinueCombo && core.CurrentStateInfo.normalizedTime >= AnimationData[step].nextStateTransitionTime && core.CurrentStateInfo.IsName(AnimationData[step].animationName))
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

                    canContinueCombo = false;
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
            core.Animator.speed = 1;
            canContinueCombo = false;
        }
    }
}
