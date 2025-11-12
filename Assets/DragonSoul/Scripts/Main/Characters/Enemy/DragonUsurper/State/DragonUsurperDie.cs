namespace Enemy
{
    public class DragonUsurperDie : IState<DragonUsurperStateID>
    {
        public DragonUsurperStateID StateID => DragonUsurperStateID.Die;
        readonly DragonUsurperCore core;

        public DragonUsurperDie(DragonUsurperCore core)
        {
            this.core = core;
        }

        public void Enter()
        {
            core.Animator.CrossFade("Die", 0);
        }

        public void Update() { }       

        public void FixedUpdate() { }

        public void Exit() { }    
    }
}

