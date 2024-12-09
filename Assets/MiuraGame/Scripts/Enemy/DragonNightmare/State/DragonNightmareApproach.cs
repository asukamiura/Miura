using System.Globalization;
using UnityEngine;

namespace Enemy
{
    public class DragonNightmareApproach : IState<DragonNightmareStateID>
    {
        public DragonNightmareStateID StateID => DragonNightmareStateID.Approach;
        private DragonNightmareCore core;
        private Vector3 playerPos;
        private float targetDistance = 3;
        private float moveSpeed = 20;

        public DragonNightmareApproach(DragonNightmareCore core)
        {
            this.core = core;
        }

        public void Enter()
        {
            core.animator.CrossFade("Run", 0.1f);
            playerPos = core.playerTransform.position;
            playerPos.y = 0;
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
                        core.stateMachine.ChangeState(DragonNightmareStateID.Attack1);
                        break;
                    case 2:
                        core.stateMachine.ChangeState(DragonNightmareStateID.Attack2);
                        break;
                    case 3:
                        core.stateMachine.ChangeState(DragonNightmareStateID.Attack3);
                        break;
                }
            }
            else
            {
                core.transform.position = Vector3.MoveTowards(core.transform.position, playerPos, moveSpeed * Time.deltaTime);              

                if (currentDistance <= targetDistance)
                {
                    core.stateMachine.ChangeState(DragonNightmareStateID.Search);
                }
            }
        }

        public void FixedUpdate() { }

        public void Exit()
        {

        }
    }

}


