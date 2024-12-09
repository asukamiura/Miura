using UnityEngine;

namespace Enemy
{
    public class DragonUsurperFlyAttack: IState<DragonUsurperStateID>
    {
        public DragonUsurperStateID StateID => DragonUsurperStateID.FlyAttack;
        private DragonUsurperCore core;
        private Vector3 playerPos = Vector3.zero;
        private float distanceToPlayer = 10;
        private Vector3 attackPoint1Pos = Vector3.zero;
        private Vector3 attackPoint2Pos = Vector3.zero;
        private Vector3 attackPoint3Pos = Vector3.zero;
        private float distance = 0;
        private float moveSpeed = 0;

        public DragonUsurperFlyAttack(DragonUsurperCore core)
        {
            this.core = core;
        }

        public void Enter()
        {
            if (core.isFlying)
            {
                core.animator.CrossFade("FlyAttack", 0);
            }

            core.IncreaseAttackCount(3);
            playerPos = core.playerTransform.position;
            attackPoint1Pos = playerPos - (core.transform.right + core.transform.forward).normalized * distanceToPlayer;
            attackPoint2Pos = playerPos + core.transform.forward * distanceToPlayer;
            distance = Vector3.Distance(core.transform.position, attackPoint1Pos);
        }

        public void Update()
        {

            AnimatorStateInfo stateInfo = core.animator.GetCurrentAnimatorStateInfo(0);
            float animationLength = stateInfo.length;
            if (stateInfo.IsName("FlyAttack"))
            {
                if (stateInfo.normalizedTime >= 1)
                {
                    core.stateMachine.ChangeState(DragonUsurperStateID.Land);
                }
            }

            moveSpeed = distance / animationLength;

            if (core.transform.position != attackPoint1Pos)
            {
                Vector3 direction = (core.playerTransform.position - core.transform.position).normalized;
                Quaternion lookAtRotation = Quaternion.LookRotation(direction, Vector3.up);
                core.transform.rotation = Quaternion.Slerp(core.transform.rotation, lookAtRotation, core.rotationSpeed * Time.deltaTime);

                core.transform.position = Vector3.MoveTowards(core.transform.position, attackPoint1Pos, moveSpeed * Time.deltaTime);
            }
        }

        public void FixedUpdate() { }
       
        public void Exit() 
        {
        }
    }
}
