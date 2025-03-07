using UnityEngine;

namespace Enemy
{
    public class DragonNightmareTakeWarning : IState<DragonNightmareStateID>
    {
        public DragonNightmareStateID StateID => DragonNightmareStateID.TakeWarning;
        DragonNightmareCore core;
        float currentTime = 0;  // 警戒時間を取得
        float warningTime = 3;
        float moveSpeed = 1;
        int moveDirection;
        const int WalkBackNum = 0;
        const int WalkRightNum = 1;
        const int WalkLeftNum = 2;

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

        public void StateUpdate() { }

        public void StateFixedUpdate()
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

