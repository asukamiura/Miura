using UnityEngine;

namespace Enemy
{
    public class DragonUsurperSearch : IState<DragonUsurperStateID>
    {
        public DragonUsurperStateID StateID => DragonUsurperStateID.Search;
        private DragonUsurperCore core;
        private const int JujgeDistance = 5;    //接近するか、後退するかのボーダー距離
        private const int AttackSpecialHealth = 250;    // 特殊攻撃を発動条件

        public DragonUsurperSearch(DragonUsurperCore core)
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
                // 視野にプレイヤーが入るまで回転
                Vector3 direction = (core.playerTransform.position - core.transform.position).normalized;
                Quaternion lookAtRotation = Quaternion.LookRotation(direction, Vector3.up);
                core.transform.rotation = Quaternion.Slerp(core.transform.rotation, lookAtRotation, core.rotationSpeed * Time.deltaTime);
            }
            else
            {
                //if (core.healthManager.HP <= AttackSpecialHealth)
                //{
                //    core.stateMachine.ChangeState(DragonUsurperStateID.TakeOff);
                //}
                core.stateMachine.ChangeState(DragonUsurperStateID.Attack);

                //if (core.DistanceToPlayer <= 5)
                //{
                //    core.stateMachine.ChangeState(DragonUsurperStateID.Leave);
                //}
                //else
                //{
                //    core.stateMachine.ChangeState(DragonUsurperStateID.Approach);
                //}
            }
        }

        public void Exit() { }
    }
}

