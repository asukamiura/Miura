using UnityEngine;

namespace Enemy
{
    public class DragonNightmareIdle : IState<DragonNightmareStateID>
    {
        public DragonNightmareStateID StateID => DragonNightmareStateID.Idle;
        private DragonNightmareCore core;
        private float currentTime = 0;
        private const float restTime = 1.5f;
        private const int resetPosY = 0;

        public DragonNightmareIdle(DragonNightmareCore core)
        {
            this.core = core;
        }

        public void Enter()
        {
            core.Animator.CrossFade("Idle", 0.1f);
            core.transform.position = new Vector3(core.transform.position.x, resetPosY, core.transform.position.z);
        }

        public void Update()
        {
            currentTime += Time.deltaTime;

            if (currentTime >= restTime)
            {
                core.stateMachine.ChangeState(DragonNightmareStateID.Move);
            }
        }

        public void FixedUpdate() { }

        public void Exit()
        {
            currentTime = 0;
        }
    }
}

