using UnityEngine;

namespace Enemy
{
    public class DragonUsurperCore : EnemyCoreBase
    {
        private const int fov = 10;     // 視野角
        private const int minSightDistance = 2;

        public Transform playerTransform;
        public StateMachine<DragonUsurperStateID> stateMachine;
        public float AngleToPlayer { get; private set; }
        public float DistanceToPlayer { get; private set; }
        public Vector3 CrossProduct { get; private set; }
        public bool IsPlayerInSight => AngleToPlayer <= fov && DistanceToPlayer >= minSightDistance;
        public float rotationSpeed = 10;
        public float rotationAngle = 10;
        public int Attack1Count { get; private set; } = 0;    // 連続攻撃1をした数
        public int Attack2Count { get; private set; } = 0;    // 連続攻撃2をした数
        public int Attack3Count { get; private set; } = 0;    // 連続攻撃3をした数
        public GameObject attack1Collider;
        public GameObject attack2Collider;
        public GameObject attack3Collider;

        private void Awake()
        {
            stateMachine = new StateMachine<DragonUsurperStateID>();
            stateMachine.RegisterState(new DragonUsurperIdle(this));
            stateMachine.RegisterState(new DragonUsurperTakeWarning(this));
            stateMachine.RegisterState(new DragonUsurperSearch(this));
            stateMachine.RegisterState(new DragonUsurperApproach(this));
            stateMachine.RegisterState(new DragonUsurperTakeOff(this));
            stateMachine.RegisterState(new DragonUsurperLand(this));
            stateMachine.RegisterState(new DragonUsurperRetreat(this));
            stateMachine.RegisterState(new DragonUsurperAttack1(this));
            stateMachine.RegisterState(new DragonUsurperAttack2(this));
            stateMachine.RegisterState(new DragonUsurperAttack3(this));
            stateMachine.RegisterState(new DragonUsurperDie(this));
        }

        private void Start()
        {
            stateMachine.Initialize(DragonUsurperStateID.TakeOff);

            attack1Collider.SetActive(false);
            attack2Collider.SetActive(false);
            attack3Collider.SetActive(false);
        }

        private void Update()
        {
            if (healthManager.isDead)
            {
                stateMachine.ChangeState(DragonUsurperStateID.Die);
            }

            stateMachine.Update();

            // プレイヤー方向の角度を計算
            Vector3 direction = (playerTransform.position - transform.position).normalized;
            AngleToPlayer = Vector3.Angle(transform.forward, direction);

            // 外積
            CrossProduct = Vector3.Cross(transform.forward, direction);

            // プレイヤーとの距離を計算
            DistanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        }

        private void FixedUpdate()
        {
            stateMachine.FixedUpdate();
        }

        public void IncreaseAttackCount(int attackType)
        {
            switch (attackType)
            {
                case 1:
                    Attack1Count++;
                    Attack2Count = 0;
                    Attack3Count = 0;
                    break;
                case 2:
                    Attack1Count = 0;
                    Attack2Count++;
                    Attack3Count = 0;
                    break;
                case 3:
                    Attack1Count = 0;
                    Attack2Count = 0;
                    Attack3Count++;
                    break;
            }
        }

        public void ResetAttackCount()
        {
            Attack1Count = 0;
            Attack2Count = 0;
            Attack3Count = 0;
        }

        public void AttackStart()
        {
            switch (stateMachine.StateID)
            {
                case DragonUsurperStateID.Attack1:
                    attack1Collider.SetActive(true);
                    break;
                case DragonUsurperStateID.Attack2:
                    attack2Collider.SetActive(true);
                    break;
                case DragonUsurperStateID.Attack3:
                    attack3Collider.SetActive(true);
                    break;
            }
        }

        public void AttackEnd()
        {
            switch (stateMachine.StateID)
            {
                case DragonUsurperStateID.Attack1:
                    attack1Collider.SetActive(false);
                    break;
                case DragonUsurperStateID.Attack2:
                    attack2Collider.SetActive(false);
                    break;
                case DragonUsurperStateID.Attack3:
                    attack3Collider.SetActive(false);
                    break;
            }
        }
    } 
}

