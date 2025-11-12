using UnityEngine;

namespace Enemy
{
    public class DragonTerrorDie : IState<DragonTerrorStateID>
    {
        public DragonTerrorStateID StateID => DragonTerrorStateID.Die;
        readonly DragonTerrorCore core;

        public DragonTerrorDie(DragonTerrorCore core)
        {
            this.core = core;
        }

        public void Enter()
        {
            core.Animator.CrossFade("Die", 0);
        }

        public void Update()
        {
           
        }

        public void FixedUpdate() { }

        public void Exit()
        {

        }
    }
}

