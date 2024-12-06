using UnityEngine;

namespace Enemy
{
    public class DragonNightmareAttack3 : IState<DragonNightmareStateID>
    {
        public DragonNightmareStateID StateID => DragonNightmareStateID.Attack3;
        private DragonNightmareCore core;
        private Vector3 playerPos;

        public DragonNightmareAttack3(DragonNightmareCore core)
        {
            this.core = core;
        }

        public void Enter()
        {
            core.animator.CrossFade("Attack3", 0);
            playerPos = core.playerTransform.position;
            core.IncreaseAttackCount(3);
        }

        public void Update()
        {        
            AnimatorStateInfo stateInfo = core.animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("Attack3"))
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
