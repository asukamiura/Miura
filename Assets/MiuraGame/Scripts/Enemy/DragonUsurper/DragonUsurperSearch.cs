using UnityEngine;

namespace Enemy
{
    public class DragonUsurperSearch : IState<DragonUsurperStateID>
    {
        public DragonUsurperStateID StateID => DragonUsurperStateID.Search;
        private DragonUsurperCore core;
        private const int attackSpecialHealth = 250;

        public DragonUsurperSearch(DragonUsurperCore core)
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
                if (core.healthManager.HP <= attackSpecialHealth)
                {
                    core.stateMachine.ChangeState(DragonUsurperStateID.TakeOff);
                }
                else if (core.DistanceToPlayer <= 5)
                {
                    core.stateMachine.ChangeState(DragonUsurperStateID.Leave);
                }
                else
                {
                    core.stateMachine.ChangeState(DragonUsurperStateID.Approach);
                }

            }
        }

        public void FixedUpdate() { }

        public void Exit()
        {

        }
    }
}

