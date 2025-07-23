using UnityEngine;

namespace Enemy
{
    public class DragonUsurperDamage : IState<DragonUsurperStateID>
    {
        public DragonUsurperStateID StateID => DragonUsurperStateID.Damage;
        
        DragonUsurperCore core;

        const float TransitionDuration = 0.1f;  // アニメーションの遷移継続時間
        const float TransitionTime = 0.8f;      // アニメーションを遷移させる時間
        const float TimeOffset = 0.3f;          

        public DragonUsurperDamage(DragonUsurperCore core)
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
                    core.stateMachine.ChangeState(DragonUsurperStateID.Idle);
                }
            }
        }

        public void FixedUpdate() { }

        public void Exit()
        {
        }
    }
}
