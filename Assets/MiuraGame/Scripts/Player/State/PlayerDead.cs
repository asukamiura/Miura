using UnityEditor;
using UnityEngine;

namespace Player
{
    public class PlayerDead : IState<PlayerStateID>
    {
        public PlayerStateID StateID => PlayerStateID.Dead;
        private InputReciver input => InputReciver.Instance;
        private PlayerCore core;

        public PlayerDead(PlayerCore core)
        {
            this.core = core;
        }

        public void Enter()
        {
            core.AttackEnd();
            core.Animator.applyRootMotion = true;
            core.Animator.CrossFade("Death", 0, 0, 0);
        }

        public void Update()
        {

        }

        public void FixedUpdate() { }

        public void Exit()
        {

        }
    }
}
