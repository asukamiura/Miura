using UnityEngine;

namespace Enemy
{
    public class DragonTerrorDamage : IState<DragonTerrorStateID>
    {
        public DragonTerrorStateID StateID => DragonTerrorStateID.Damage;

        DragonTerrorCore core;

        const float TransitionDuration = 0.1f;  // アニメーションの遷移継続時間
        const float TransitionTime = 0.8f;      // アニメーションを遷移させる時間
        const float TimeOffset = 0.3f;

        public DragonTerrorDamage(DragonTerrorCore core)
        {
            this.core = core;
        }

        public void Enter()
        {
            core.Animator.CrossFade("Damage", TransitionDuration, 0, TimeOffset);

            core.NavMeshAgent.speed = 0;
            core.NavMeshAgent.acceleration = 0;
            core.NavMeshAgent.velocity = Vector3.zero;
        }

        public void Update()
        {
            if (core.CurrentStateInfo.IsName("Damage"))
            {
                if (core.CurrentStateInfo.normalizedTime >= TransitionTime)
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
