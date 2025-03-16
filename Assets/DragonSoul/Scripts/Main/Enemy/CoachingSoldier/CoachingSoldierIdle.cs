using UnityEngine;

namespace Enemy
{
    public class CoachingSoldierIdle : IState<CoachingSoldierStateID>
    {
        public CoachingSoldierStateID StateID => CoachingSoldierStateID.Idle;
        CoachingSoldierCore core;
        float currentTime = 0;
        const float restTime = 1.5f;
        const int resetPosY = 0;

        public CoachingSoldierIdle(CoachingSoldierCore core)
        {
            this.core = core;
        }

        public void Enter()
        {
            core.animator.CrossFade("Idle", 0.1f);
            core.transform.position = new Vector3(core.transform.position.x, resetPosY, core.transform.position.z);
        }

        public void Update()
        {
            currentTime += Time.deltaTime;

            if (currentTime >= restTime)
            {
                core.stateMachine.ChangeState(CoachingSoldierStateID.TakeWarning);
            }
        }

        public void FixedUpdate() { }

        public void Exit()
        {
            currentTime = 0;
        }
    }
}

