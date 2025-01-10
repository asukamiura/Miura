using UnityEngine;

namespace Enemy
{
    public class DragonNightmareSearch : IState<DragonNightmareStateID>
    {
        public DragonNightmareStateID StateID => DragonNightmareStateID.Search;
        private DragonNightmareCore core;
        private const float attackRange = 5;

        public DragonNightmareSearch(DragonNightmareCore core)
        {
            this.core = core;
        }

        public void Enter()
        {
            core.animator.CrossFade("WalkFront", 0.1f);
        }

        public void Update() { }

        public void FixedUpdate()
        {
            if (!core.IsPlayerInSight)
            {
                core.LookAtPlayer();
            }
            else
            {
                if (core.DistanceToPlayer <= attackRange)
                {
                    core.stateMachine.ChangeState(DragonNightmareStateID.Attack);
                }
                else
                {
                    core.stateMachine.ChangeState(DragonNightmareStateID.Move);
                }

            }
        }

        public void Exit() { }
    }
}

