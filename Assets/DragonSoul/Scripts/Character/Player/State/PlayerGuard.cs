namespace Player
{
    public class PlayerGuard : IState<PlayerStateID>
    {
        public PlayerStateID StateID => PlayerStateID.Guard;
        InputReciver input => InputReciver.Instance;
        PlayerCore core;

        const float AnimationEndThreshold = 1f; // アニメーション終了のしきい値

        public PlayerGuard(PlayerCore core)
        {
            this.core = core;
        }

        public void Enter()
        {
            core.attackAssist.CorrectionAttack();
            core.Animator.CrossFade("Guard", 0);
        }

        public void Update()
        {
            float normalizedTime = core.CurrentStateInfo.normalizedTime;

            if (normalizedTime >= AnimationEndThreshold && core.CurrentStateInfo.IsName("Guard"))
            {
                core.stateMachine.ChangeState(PlayerStateID.Locomotion);
            }
        }

        public void FixedUpdate() { }

        public void Exit() { }
    }
}
