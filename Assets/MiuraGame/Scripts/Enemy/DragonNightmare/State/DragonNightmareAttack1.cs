using UnityEngine;

namespace Enemy
{
    public class DragonNightmareAttack1 : IState<DragonNightmareStateID>
    {
        public DragonNightmareStateID StateID => DragonNightmareStateID.Attack1;
        private DragonNightmareCore core;

        public DragonNightmareAttack1(DragonNightmareCore core)
        {
            this.core = core;
        }

        public void Enter()
        {
            core.animator.CrossFade("Attack1", 0);
            core.IncreaseAttackCount(1);
        }

        public void Update()
        {
            AnimatorStateInfo stateInfo = core.animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("Attack1"))
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

