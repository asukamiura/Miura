using UnityEngine;

namespace Enemy
{
    public class DragonNightmareAttack2 : IState<DragonNightmareStateID>
    {
        public DragonNightmareStateID StateID => DragonNightmareStateID.Attack2;
        private DragonNightmareCore core;

        public DragonNightmareAttack2(DragonNightmareCore core)
        {
            this.core = core;
        }

        public void Enter()
        {
            core.animator.CrossFade("Attack2", 0.2f);
            core.IncreaseAttackCount(2);
        }

        public void Update()
        {
            AnimatorStateInfo stateInfo = core.animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("Attack2"))
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

