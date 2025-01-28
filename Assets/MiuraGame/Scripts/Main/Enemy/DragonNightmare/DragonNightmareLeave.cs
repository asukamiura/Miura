using UnityEngine;

namespace Enemy
{
    public class DragonNightmareLeave : IState<DragonNightmareStateID>
    {
        public DragonNightmareStateID StateID => DragonNightmareStateID.Leave;
        DragonNightmareCore core;
        float targetDistance = 3;
        int attackType;
        Vector3 attackTargetPos;
        float leaveSpeed = 2;

        public DragonNightmareLeave(DragonNightmareCore core)
        {
            this.core = core;
        }

        public void Enter()
        {
            core.animator.CrossFade("WalkBack", 0);

            attackType = Random.Range(1, 4);
            switch (attackType)
            {
                case 1:
                    targetDistance = 1;
                    break;
                case 2:
                    targetDistance = 2;
                    break;
                case 3:
                    targetDistance = 2;
                    break;
            }

            // 移動目標地点を設定
            attackTargetPos = core.playerTransform.position - core.transform.forward * targetDistance;
        }

        public void Update() { }

        public void FixedUpdate()
        {
            if (core.transform.position == attackTargetPos)
            {
                switch (attackType)
                {
                    case 1:
                        break;
                    case 2:
                        break;
                    case 3:
                        break;
                }
            }
            else
            {
                Vector3 direction = (core.playerTransform.position - core.transform.position).normalized;
                Quaternion lookAtRotation = Quaternion.LookRotation(direction, Vector3.up);
                core.transform.rotation = Quaternion.Slerp(core.transform.rotation, lookAtRotation, core.rotationSpeed * Time.deltaTime);

                core.transform.position = Vector3.MoveTowards(core.transform.position, attackTargetPos, leaveSpeed * Time.deltaTime);
            }
        }

        public void Exit()
        {

        }
    }
}

