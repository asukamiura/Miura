using UnityEngine;

namespace Enemy
{
    public class DragonTerrorDie : IState<DragonTerrorStateID>
    {
        public DragonTerrorStateID StateID => DragonTerrorStateID.Die;
        private DragonTerrorCore core;

        public DragonTerrorDie(DragonTerrorCore core)
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

