using UnityEngine;

namespace Enemy
{
    public class DragonUsurperIdle : IState<DragonUsurperStateID>
    {
        public DragonUsurperStateID StateID => DragonUsurperStateID.Idle;
        DragonUsurperCore core;
        float currentTime = 0;
        const float restTime = 2f;

        public DragonUsurperIdle(DragonUsurperCore core)
        {
            this.core = core;
        }

        public void Enter()
        {
            core.Animator.CrossFade("Idle", 0.1f);
        }

        public void Update()
        {
            currentTime += Time.deltaTime;

            if (currentTime >= restTime)
            {
                core.stateMachine.ChangeState(DragonUsurperStateID.Move);
            }
        }

        public void FixedUpdate() { }

        public void Exit()
        {
            currentTime = 0;
        }
    }
}

