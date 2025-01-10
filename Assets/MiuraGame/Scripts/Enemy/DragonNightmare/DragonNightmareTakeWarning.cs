using UnityEngine;

namespace Enemy
{
    public class DragonNightmareTakeWarning : IState<DragonNightmareStateID>
    {
        public DragonNightmareStateID StateID => DragonNightmareStateID.TakeWarning;
        private DragonNightmareCore core;
        private float currentTime = 0;  // 警戒時間を取得
        private float warningTime = 3;
        private float moveSpeed = 1;
        private int moveDirection;
        private const int WalkBackNum = 0;        
        private const int WalkRightNum = 1;
        private const int WalkLeftNum = 2;

        public DragonNightmareTakeWarning(DragonNightmareCore core)
        {
            this.core = core;
        }

        public void Enter()
        {
            moveDirection = Random.Range(WalkBackNum, WalkLeftNum + 1);
            switch (moveDirection)
            {
                case WalkBackNum:
                    core.animator.CrossFade("WalkBack", 0);
                    break;
                case WalkRightNum:
                    core.animator.CrossFade("WalkRight", 0);
                    break;
                case WalkLeftNum:
                    core.animator.CrossFade("WalkLeft", 0);
                    break;
            }
        }

        public void Update() { }
        
        public void FixedUpdate() 
        {
            currentTime += Time.deltaTime;

            if (currentTime >= warningTime)
            {
                core.stateMachine.ChangeState(DragonNightmareStateID.Search);
            }
            else
            {
                core.LookAtPlayer();

                switch (moveDirection)
                {
                    case WalkBackNum:
                        core.transform.position -= core.transform.forward * moveSpeed * Time.deltaTime;
                        break;
                    case WalkRightNum:
                        core.transform.position += core.transform.right * moveSpeed * Time.deltaTime;
                        break;
                    case WalkLeftNum:
                        core.transform.position -= core.transform.right * moveSpeed * Time.deltaTime;
                        break;
                }
            }
        }

        public void Exit()
        {
            currentTime = 0;
        }
    }
}

