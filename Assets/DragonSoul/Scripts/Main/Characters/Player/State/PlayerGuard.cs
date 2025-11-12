namespace Player
{
    public class PlayerGuard : IState<PlayerStateID>
    {
        public PlayerStateID StateID => PlayerStateID.Guard;
        readonly PlayerCore core;
        readonly AttackAssist attackAssist;

        const float AnimationEndThreshold = 1f; // アニメーション終了のしきい値

        public PlayerGuard(PlayerCore core, AttackAssist attackAssist)
        {
            this.core = core;
            this.attackAssist = attackAssist;
        }

        public void Enter()
        {
            attackAssist.CorrectionAttack();
            core.Animator.CrossFade("Guard", 0);
        }

        public void Update()
        {
            float normalizedTime = core.CurrentStateInfo.normalizedTime;

            if (normalizedTime >= AnimationEndThreshold && core.CurrentStateInfo.IsName("Guard"))
            {
                core.StateMachine.ChangeState(PlayerStateID.Locomotion);
            }
        }

        public void FixedUpdate() { }

        public void Exit() { }
    }
}
