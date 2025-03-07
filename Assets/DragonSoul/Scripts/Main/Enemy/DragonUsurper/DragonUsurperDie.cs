using UnityEngine;

namespace Enemy
{
    public class DragonUsurperDie : IState<DragonUsurperStateID>
    {
        public DragonUsurperStateID StateID => DragonUsurperStateID.Die;
        DragonUsurperCore core;

        public DragonUsurperDie(DragonUsurperCore core)
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

