using UnityEngine;

namespace Player
{
    public class PlayerAttackUltimate : IState<PlayerStateID>
    {
        readonly PlayerCore core;
        readonly AnimationController animationController;
        readonly PlayerAttackUltimateController ultimateAttackController;
        PlayerAttackData attackData;

        public PlayerStateID StateID => PlayerStateID.AttackUltimate;

        public PlayerAttackUltimate(PlayerCore core, AnimationController animationController, PlayerAttackUltimateController ultimateAttackController)
        {
            this.core = core;
            this.animationController = animationController;
            this.ultimateAttackController = ultimateAttackController;
        }

        public void Enter()
        {
            core.IsInvincible = true;
            core.Rb.velocity = Vector3.zero;
            ultimateAttackController.OnHitDetectionPerformed += HandlePerformHit;
            attackData = ultimateAttackController.GetAttackData();
            animationController.PlayAniamtion(attackData.AnimationStateName, attackData.TransitionDuration, 0, attackData.TimeOffset);
        }

        public void Update()
        {
            if (animationController.IsTimeElapsed(attackData.AnimationStateName, attackData.TransitionTime))
            {
                core.StateMachine.ChangeState(PlayerStateID.Locomotion);
            }
        }

        public void FixedUpdate() { }

        public void Exit()
        {
            core.IsInvincible = false;
            ultimateAttackController.OnHitDetectionPerformed -= HandlePerformHit;
        }

        void HandlePerformHit()
        {
            ultimateAttackController.ExecuteAttack(attackData, core.AttackPower, core.InPowerUp, core.CenterTransform, animationController.Animator);
        }
    }
}
