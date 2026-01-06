namespace Player
{
    public class PlayerDead : IState<PlayerStateID>
    {
        public PlayerStateID StateID => PlayerStateID.Dead;
        readonly PlayerCore core;
        readonly AnimationController animationController;
        const string AnimationStateName = "Death";

        public PlayerDead(PlayerCore core, AnimationController animationController)
        {
            this.core = core;
            this.animationController = animationController;
        }

        public void Enter()
        {
            animationController.PlayAniamtion(AnimationStateName);
            animationController.SetRootMotion(true);
        }

        public void Update() { }    

        public void FixedUpdate() { }

        public void Exit() { }     
    }
}
