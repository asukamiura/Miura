using UnityEngine;

namespace Enemy
{
    public class CoachingSoldierIdle : IState<CoachingSoldierStateID>
    {
        public CoachingSoldierStateID StateID => CoachingSoldierStateID.Idle;
        readonly CoachingSoldierCore core;
        float currentTime = 0;
        const float RestTime = 1.5f;
        const int ResetPosY = 0;

        public CoachingSoldierIdle(CoachingSoldierCore core)
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

