using UnityEngine;

namespace Enemy
{
    public class DragonNightmareIdle : IState<DragonNightmareStateID>
    {
        public DragonNightmareStateID StateID => DragonNightmareStateID.Idle;
        readonly DragonNightmareCore core;
        float currentTime = 0;
        const float RestTime = 1.5f;
        const int ResetPosY = 0;

        public DragonNightmareIdle(DragonNightmareCore core)
        {
            this.core = core;
        }

        public void Enter()
        {
            core.Animator.CrossFade("Idle", 0.1f);
            core.transform.position = new Vector3(core.transform.position.x, ResetPosY, core.transform.position.z);
        }

        public void Update()
        {
            currentTime += Time.deltaTime;

            if (currentTime >= RestTime)
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

