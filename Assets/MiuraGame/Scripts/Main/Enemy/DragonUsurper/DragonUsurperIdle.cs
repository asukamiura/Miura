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
            core.animator.CrossFade("Idle", 0.1f);
        }

        public void StateUpdate()
        {
            currentTime += Time.deltaTime;

            if (currentTime >= restTime)
            {
                core.stateMachine.ChangeState(DragonUsurperStateID.Move);
            }
        }

        public void StateFixedUpdate() { }

        public void Exit()
        {
            currentTime = 0;
        }
    }
}

