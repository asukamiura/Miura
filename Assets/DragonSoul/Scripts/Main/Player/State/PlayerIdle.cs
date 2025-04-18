using UnityEngine;

namespace Player
{
    public class PlayerIdle : IState<PlayerStateID>
    {
        public PlayerStateID StateID => PlayerStateID.Locomotion;
        InputReciver Input => InputReciver.Instance;
        PlayerCore core;

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

            if (core.CurrentStateInfo.IsName("Locomotion"))
            {
                if (Input.Dash)
                {
                    core.stateMachine.ChangeState(PlayerStateID.Dash);
                }

                if (Input.AttackNormal)
                {
                    core.stateMachine.ChangeState(PlayerStateID.AttackNormal);
                }

                //if (Input.Move != Vector2.zero)
                //{
                //    core.stateMachine.ChangeState(PlayerStateID.Move);
                //}
            }

        }

        public void FixedUpdate() { }

        public void Exit()
        {

        }
    }
}
