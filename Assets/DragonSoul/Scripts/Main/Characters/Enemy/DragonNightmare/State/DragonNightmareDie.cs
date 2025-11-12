namespace Enemy
{
    public class DragonNightmareDie : IState<DragonNightmareStateID>
    {
        public DragonNightmareStateID StateID => DragonNightmareStateID.Die;
        readonly DragonNightmareCore core;

        public DragonNightmareDie(DragonNightmareCore core)
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

