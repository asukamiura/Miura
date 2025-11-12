using UnityEngine;

namespace Enemy
{
    public class DragonTerrorIdle : IState<DragonTerrorStateID>
    {
        public DragonTerrorStateID StateID => DragonTerrorStateID.Idle;
        readonly DragonTerrorCore core;
        float currentTime = 0;
        const float restTime = 3f;

        public DragonTerrorIdle(DragonTerrorCore core)
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
                core.stateMachine.ChangeState(DragonTerrorStateID.Move);
            }
        }

        public void FixedUpdate() { }

        public void Exit()
        {
            currentTime = 0;
        }
    }
}

