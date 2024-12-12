using UnityEngine;

namespace Player
{
    public class PlayerIdle : IState<PlayerStateID>
    {
        public PlayerStateID StateID => PlayerStateID.Idle;
        private InputReciver Input => InputReciver.Instance;
        private PlayerCore core;

        public PlayerIdle(PlayerCore core)
        {
            this.core = core;
        }

        public void Enter()
        {
            core.Animator.applyRootMotion = false;
            core.Animator.CrossFade("Locomotion", 0.2f, 0, 0);
        }

        public void Update()
        {
            core.Rb.velocity = Vector3.zero;

            if (Input.Dodge)
            {
                core.stateMachine.ChangeState(PlayerStateID.Dodge);
            }

            if (Input.AttackCharge)
            {
                core.justPointManager.UseJustPoints(1);
                core.stateMachine.ChangeState(PlayerStateID.AttackCharge);
                Input.ResetInputCountTime();
            }
            else if (Input.AttackNormal)
            {
                core.stateMachine.ChangeState(PlayerStateID.AttackNormal1);
                Input.ResetInputCountTime();
            }

            if (Input.Move != Vector2.zero)
            {
                core.stateMachine.ChangeState(PlayerStateID.Move);
            }
        }

        public void FixedUpdate() { }

        public void Exit()
        {

        }
    }
}
