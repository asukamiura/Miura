using UnityEngine;

namespace Enemy
{
    public class CoachingSoldierCore : EnemyCoreBase
    {
        private float previousHP;

        private const int fov = 10;     // 視野角
        private const int minSightDistance = 2;
        private const int minHitCount = 0;
        private const int maxHitCount = 3;

        public Transform playerTransform;
        public TutorialManager tutorialManager;
        public StateMachine<CoachingSoldierStateID> stateMachine;
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
        public int hitCount = 0;    // 連続で攻撃を受けた回数
        public bool CanAttack1 => tutorialManager.currentTask is JustGaurdTask; 
        public bool CanAttack2 => tutorialManager.currentTask is JustDodgeTask; 

        private void Awake()
        {
            stateMachine = new StateMachine<CoachingSoldierStateID>();
            stateMachine.RegisterState(new CoachingSoldierIdle(this));
            stateMachine.RegisterState(new CoachingSoldierApproach(this));
            stateMachine.RegisterState(new CoachingSoldierTakeWarning(this));
            stateMachine.RegisterState(new CoachingSoldierAttack1(this));
            stateMachine.RegisterState(new CoachingSoldierAttack2(this));
            stateMachine.RegisterState(new CoachingSoldierDamage(this));
        }

        private void Start()
        {
            stateMachine.Initialize(CoachingSoldierStateID.Idle);

            attack1Collider.SetActive(false);
            attack2Collider.SetActive(false);
        }

        private void Update()
        {
            if (healthManager.isDead)
            {
                stateMachine.ChangeState(CoachingSoldierStateID.Die);
            }

            stateMachine.Update();

            // プレイヤー方向の角度を計算
            Vector3 direction = (playerTransform.position - transform.position).normalized;
            AngleToPlayer = Vector3.Angle(transform.forward, direction);

            // 外積
            CrossProduct = Vector3.Cross(transform.forward, direction);

            // プレイヤーとの距離を計算
            DistanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

            if (healthManager.HP != previousHP)
            {
                healthManager.Heal(100);
                previousHP = healthManager.HP;
            }

            hitCount = Mathf.Clamp(hitCount, minHitCount, maxHitCount);
        }

        private void FixedUpdate()
        {
            stateMachine.FixedUpdate();
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Sword"))
            {
                stateMachine.ChangeState(CoachingSoldierStateID.Damage);
            }
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
                case CoachingSoldierStateID.Attack1:
                    attack1Collider.SetActive(true);
                    break;
                case CoachingSoldierStateID.Attack2:
                    attack2Collider.SetActive(true);
                    break;
                case CoachingSoldierStateID.Attack3:
                    break;
            }
        }

        public void AttackEnd()
        {
            switch (stateMachine.StateID)
            {
                case CoachingSoldierStateID.Attack1:
                    attack1Collider.SetActive(false);
                    break;
                case CoachingSoldierStateID.Attack2:
                    attack2Collider.SetActive(false);
                    break;
                case CoachingSoldierStateID.Attack3:
                    break;
            }
        }
    } 
}

