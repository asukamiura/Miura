using UnityEngine;

namespace Enemy
{
    public class DragonNightmareDamage : IState<DragonNightmareStateID>
    {
        public DragonNightmareStateID StateID => DragonNightmareStateID.Damage;
        private DragonNightmareCore core;

        public DragonNightmareDamage(DragonNightmareCore core)
        {
            this.core = core;
        }

        public void Enter()
        {
            core.animator.CrossFade("Damage", 0.1f, 0, 0);
        }

        public void Update()
        {        
            AnimatorStateInfo stateInfo = core.animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("Damage"))
            {
                if (stateInfo.normalizedTime >= 1)
                {
                    core.stateMachine.ChangeState(DragonNightmareStateID.Idle);
                }
            }
        }

        public void FixedUpdate() { }
       
        public void Exit() 
        {
        }
    }
}
