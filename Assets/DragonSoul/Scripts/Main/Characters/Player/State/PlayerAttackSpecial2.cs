namespace Player
{
    public class PlayerAttackSpecial2 : IState<PlayerStateID>
    {
        readonly PlayerCore core;
        readonly AnimationController animationController;
        readonly PlayerAttackSpecial2Controller specialAttack2Controller;
        PlayerAttackData currentAttackData;
        int currentStep = 1;
        InputReceiver Input => InputReceiver.Instance;

        public PlayerStateID StateID => PlayerStateID.AttackSpecial2;

        public PlayerAttackSpecial2(PlayerCore core, AnimationController animationController, PlayerAttackSpecial2Controller specialAttack2Controller)
        {
            this.core = core;
            this.animationController = animationController;
            this.specialAttack2Controller = specialAttack2Controller;
        }

        public void Enter()
        {
            core.IsInvincible = true;
            PlayAnimation();
            animationController.SetRootMotion(true);
            specialAttack2Controller.OnHitDetectionPerformed += HandlePerformHit;
        }

        public void Update()
        {
            // 現在の段が最終段か？
            bool isLastStep = currentStep >= specialAttack2Controller.GetMaxComboCount();

            // ガードステートに遷移
            if (Input.Guard)
            {
                core.StateMachine.ChangeState(PlayerStateID.Guard);
                return;
            }

            // ダッシュステートに遷移
            if (Input.Dash)
            {
                core.StateMachine.ChangeState(PlayerStateID.Dash);
                return;
            }

            if (!isLastStep && animationController.IsTimeElapsed(currentAttackData.AnimationStateName, currentAttackData.TransitionTime))
            {
                currentStep++;
                PlayAnimation();
                return;
            }

            if (isLastStep && animationController.IsTimeElapsed(currentAttackData.AnimationStateName, currentAttackData.TransitionTime))
            {
                core.StateMachine.ChangeState(PlayerStateID.Locomotion);
            }
        }

        public void FixedUpdate() { }

        public void Exit()
        {
            currentStep = 1;
            animationController.SetRootMotion(false);
            core.IsInvincible = false;
            specialAttack2Controller.OnHitDetectionPerformed -= HandlePerformHit;
        }

        // アニメーションを再生
        void PlayAnimation()
        {
            currentAttackData = specialAttack2Controller.GetCurrentAttackData(currentStep);

            animationController.PlayAniamtion(currentAttackData.AnimationStateName, currentAttackData.TransitionDuration, 0, currentAttackData.TimeOffset);
        }

        void HandlePerformHit()
        {
            specialAttack2Controller.ExecuteAttack(currentAttackData, core.AttackPower, core.InPowerUp, core.CenterTransform, animationController.Animator);
        }
    }
}
