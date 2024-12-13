using System.Globalization;
using UnityEngine;

namespace Enemy
{
    public class DragonNightmareApproach : IState<DragonNightmareStateID>
    {
        public DragonNightmareStateID StateID => DragonNightmareStateID.Approach;
        private DragonNightmareCore core;
        private float targetDistance = 3;
        private float moveSpeed = 20;
        private int attackType;
        private Vector3 attackTargetPos;

        public DragonNightmareApproach(DragonNightmareCore core)
        {
            this.core = core;
        }

        public void Enter()
        {
            core.animator.CrossFade("Run", 0.1f);

            attackType = Random.Range(1, 4);
            switch (attackType)
            {
                case 1:
                    targetDistance = 2;
                    break;
                case 2:
                    targetDistance = 3;
                    break;
                case 3:
                    targetDistance = 2;
                    break;
            }

            // 攻撃開始地点を設定
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
                // プレイヤーの方向を向く
                Vector3 direction = (core.playerTransform.position - core.transform.position).normalized;
                Quaternion lookAtRotation = Quaternion.LookRotation(direction, Vector3.up);
                core.transform.rotation = Quaternion.Slerp(core.transform.rotation, lookAtRotation, core.rotationSpeed * Time.deltaTime);

                // 攻撃開始地点まで移動
                core.transform.position = Vector3.MoveTowards(core.transform.position, attackTargetPos, moveSpeed * Time.deltaTime);
            }
        }

        public void Exit()
        {

        }
    }

}


