using UnityEngine;

namespace Enemy
{
    public class DragonUsurperAttack3 : IState<DragonUsurperStateID>
    {
        public DragonUsurperStateID StateID => DragonUsurperStateID.Attack3;
        private DragonUsurperCore core;
        private Vector3 playerPos;

        public DragonUsurperAttack3(DragonUsurperCore core)
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
                    core.stateMachine.ChangeState(DragonUsurperStateID.Idle);
                }
            }
        }

        public void FixedUpdate() { }
       
        public void Exit() 
        {
        }
    }
}
