using UnityEngine;

namespace Enemy
{
    public class DragonNightmareSearch : IState<DragonNightmareStateID>
    {
        public DragonNightmareStateID StateID => DragonNightmareStateID.Search;
        private DragonNightmareCore core;

        public DragonNightmareSearch(DragonNightmareCore core)
        {
            this.core = core;
        }

        public void Enter()
        {
            core.animator.CrossFade("WalkFront", 0.1f);
        }

        public void Update()
        {
            if (!core.IsPlayerInSight)
            {
                Vector3 direction = (core.playerTransform.position - core.transform.position).normalized;
                Quaternion lookAtRotation = Quaternion.LookRotation(direction, Vector3.up);
                core.transform.rotation = Quaternion.Slerp(core.transform.rotation, lookAtRotation, core.rotationSpeed * Time.deltaTime);
            }
            else
            {
                if (core.Attack1Count >= 2 || core.Attack2Count >= 2 || core.Attack3Count >= 2)
                {
                    core.stateMachine.ChangeState(DragonNightmareStateID.Retreat);
                }
                else
                {
                    core.stateMachine.ChangeState(DragonNightmareStateID.Approach);
                }

            }
        }

        public void FixedUpdate() { }

        public void Exit()
        {

        }
    }
}

