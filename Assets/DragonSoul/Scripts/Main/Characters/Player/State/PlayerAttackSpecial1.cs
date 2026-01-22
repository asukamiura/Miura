namespace Player
{
    public class PlayerAttackSpecial1 : IState<PlayerStateID>
    {
        readonly PlayerCore core;
        readonly AnimationController animationController;
        readonly PlayerAttackSpecial1Controller specialAttack1Controller;
        PlayerAttackData currentAttackData;
        int currentStep = 1;
        InputReceiver Input => InputReceiver.Instance;

        public PlayerStateID StateID => PlayerStateID.AttackSpecial1;

        public PlayerAttackSpecial1(PlayerCore core, AnimationController animationController, PlayerAttackSpecial1Controller specialAttack1Controller)
        {
            this.core = core;
            this.animationController = animationController;
            this.specialAttack1Controller = specialAttack1Controller;
        }

        public void Enter()
        {
            core.IsInvincible = true;
            PlayAnimation();
            animationController.SetRootMotion(true);
            specialAttack1Controller.OnHitDetectionPerformed += HandlePerformHit;
        }

        public void Update()
        {
            // 現在の段が最終段か？
            bool isLastStep = currentStep >= specialAttack1Controller.GetMaxComboCount();

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
            specialAttack1Controller.OnHitDetectionPerformed -= HandlePerformHit;
        }

        // アニメーションを再生
        void PlayAnimation()
        {
            currentAttackData = specialAttack1Controller.GetCurrentAttackData(currentStep);

            animationController.PlayAniamtion(currentAttackData.AnimationStateName, currentAttackData.TransitionDuration, 0, currentAttackData.TimeOffset);
        }

        void HandlePerformHit()
        {
            specialAttack1Controller.ExecuteAttack(currentAttackData, core.AttackPower, core.InPowerUp, core.CenterTransform, animationController.Animator);
        }
    }
}
