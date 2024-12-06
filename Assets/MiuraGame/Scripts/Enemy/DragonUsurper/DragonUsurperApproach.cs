using System.Globalization;
using UnityEngine;

namespace Enemy
{
    public class DragonUsurperApproach : IState<DragonUsurperStateID>
    {
        public DragonUsurperStateID StateID => DragonUsurperStateID.Approach;
        private DragonUsurperCore core;
        private Vector3 playerPos;
        private float targetDistance = 3;
        private float moveSpeed = 30;

        public DragonUsurperApproach(DragonUsurperCore core)
        {
            this.core = core;
        }

        public void Enter()
        {
            core.animator.CrossFade("Run", 0.1f);
            playerPos = core.playerTransform.position;
        }

        public void Update()
        {
            float currentDistance = Vector3.Distance(core.transform.position,playerPos);
            if (core.DistanceToPlayer <= 4)
            {
                int num = Random.Range(1, 4);
                switch (num)
                {
                    case 1:
                        core.stateMachine.ChangeState(DragonUsurperStateID.Attack1);
                        break;
                    case 2:
                        core.stateMachine.ChangeState(DragonUsurperStateID.Attack2);
                        break;
                    case 3:
                        core.stateMachine.ChangeState(DragonUsurperStateID.Attack3);
                        break;
                }
            }
            else
            {
                Vector3 targetPosition = Vector3.MoveTowards(core.transform.position, playerPos, moveSpeed * Time.deltaTime);
                core.rb.MovePosition(targetPosition);

                if (currentDistance <= targetDistance)
                {
                    core.stateMachine.ChangeState(DragonUsurperStateID.Search);
                }
            }
        }

        public void FixedUpdate() { }

        public void Exit()
        {

        }
    }

}


