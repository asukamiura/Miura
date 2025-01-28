using UnityEngine;

namespace Enemy
{
    public class DragonTerrorDamage : IState<DragonTerrorStateID>
    {
        public DragonTerrorStateID StateID => DragonTerrorStateID.Damage;
        DragonTerrorCore core;

        public DragonTerrorDamage(DragonTerrorCore core)
        {
            this.core = core;
        }

        public void Enter()
        {
            core.animator.CrossFade("Damage", 0.1f, 0, 0.2f);

            core.navMeshAgent.speed = 0;
            core.navMeshAgent.acceleration = 0;
            core.navMeshAgent.velocity = Vector3.zero;
        }

        public void Update()
        {
            AnimatorStateInfo stateInfo = core.animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("Damage"))
            {
                if (stateInfo.normalizedTime >= 1)
                {
                    core.stateMachine.ChangeState(DragonTerrorStateID.Idle);
                }
            }
        }

        public void FixedUpdate() { }

        public void Exit()
        {
        }
    }
}
