namespace Player
{
    public class PlayerDead : IState<PlayerStateID>
    {
        public PlayerStateID StateID => PlayerStateID.Dead;
        InputReciver input => InputReciver.Instance;
        PlayerCore core;

        public PlayerDead(PlayerCore core)
        {
            this.core = core;
        }

        public void Enter()
        {
            core.Animator.applyRootMotion = true;
            core.Animator.CrossFade("Death", 0, 0, 0);
        }

        public void StateUpdate()
        {

        }

        public void StateFixedUpdate() { }

        public void Exit()
        {

        }
    }
}
