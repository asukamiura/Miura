using UnityEngine;

namespace Enemy
{
    public class DragonNightmareDie : IState<DragonNightmareStateID>
    {
        public DragonNightmareStateID StateID => DragonNightmareStateID.Die;
        private DragonNightmareCore core;

        public DragonNightmareDie(DragonNightmareCore core)
        {
            this.core = core;
        }

        public void Enter()
        {
            core.animator.CrossFade("Die", 0);
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

