using Unity.VisualScripting;
using UnityEngine;

namespace Enemy
{
    public class DragonUsurperIdle : IState<DragonUsurperStateID>
    {
        public DragonUsurperStateID StateID => DragonUsurperStateID.Idle;
        private DragonUsurperCore core;
        private float currentTime = 0;
        private const float restTime = 1.5f;

        public DragonUsurperIdle(DragonUsurperCore core)
        {
            this.core = core;
        }

        public void Enter()
        {
            core.animator.CrossFade("Idle", 0.1f);
        }

        public void Update()
        {
            currentTime += Time.deltaTime;

            if (currentTime >= restTime)
            {
                core.stateMachine.ChangeState(DragonUsurperStateID.TakeWarning);
            }
        }

        public void FixedUpdate() { }

        public void Exit()
        {
            currentTime = 0;
        }
    }
}

