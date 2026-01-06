namespace Player
{
    public class PlayerGuard : IState<PlayerStateID>
    {
        public PlayerStateID StateID => PlayerStateID.Guard;
        readonly PlayerCore core;
        readonly AnimationController animationController;
        readonly AttackAssist attackAssist;

        const string AnimationStateName = "Guard";
        const float TransitionThreshold = 1.0f;     // 遷移を開始するアニメーションの進捗率

        public PlayerGuard(PlayerCore core, AnimationController animationController, AttackAssist attackAssist)
        {
            this.core = core;
            this.animationController = animationController;
            this.attackAssist = attackAssist;
        }

        public void Enter()
        {
            attackAssist.CorrectionAttack();
            animationController.PlayAniamtion(AnimationStateName);
        }

        public void Update()
        {
            if (animationController.IsTimeElapsed(AnimationStateName, TransitionThreshold))
            {
                core.StateMachine.ChangeState(PlayerStateID.Locomotion);
            }
        }

        public void FixedUpdate() { }

        public void Exit() { }
    }
}
