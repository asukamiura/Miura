namespace Player
{
    public class PlayerDead : IState<PlayerStateID>
    {
        public PlayerStateID StateID => PlayerStateID.Dead;
        readonly PlayerCore core;

        public PlayerDead(PlayerCore core)
        {
            this.core = core;
        }

        public void Enter()
        {
            core.Animator.applyRootMotion = true;
            core.Animator.CrossFade("Death", 0);
        }

        public void Update() { }    

        public void FixedUpdate() { }

        public void Exit() { }     
    }
}
