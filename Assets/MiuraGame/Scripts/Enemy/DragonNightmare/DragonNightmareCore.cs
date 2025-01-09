using Player;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Enemy
{
    public class DragonNightmareCore : EnemyCoreBase
    {
        [SerializeField] private List<GameObject> attackColliders = new List<GameObject>();

        private const int Fov = 10;     // 視野角
        private const int MinSightDistance = 2;

        public Transform playerTransform;
        public StateMachine<DragonNightmareStateID> stateMachine;
        public float AngleToPlayer { get; private set; }    // プレイヤーのいる角度
        public float DistanceToPlayer { get; private set; } // プレイヤーとの距離
        public Vector3 CrossProduct { get; private set; }
        public bool IsPlayerInSight => AngleToPlayer <= Fov && DistanceToPlayer >= MinSightDistance;
        public float rotationSpeed = 10;
        public float rotationAngle = 10;
        public int Attack1Count { get; private set; } = 0;    // 連続攻撃1をした数
        public int Attack2Count { get; private set; } = 0;    // 連続攻撃2をした数
        public int Attack3Count { get; private set; } = 0;    // 連続攻撃3をした数

        private void Awake()
        {
            stateMachine = new StateMachine<DragonNightmareStateID>();
            stateMachine.RegisterState(new DragonNightmareIdle(this));
            stateMachine.RegisterState(new DragonNightmareTakeWarning(this));
            stateMachine.RegisterState(new DragonNightmareSearch(this));
            stateMachine.RegisterState(new DragonNightmareLeave(this));
            stateMachine.RegisterState(new DragonNightmareApproach(this));
            stateMachine.RegisterState(new DragonNightmareRetreat(this));
            stateMachine.RegisterState(new DragonNightmareAttack(this));            
            stateMachine.RegisterState(new DragonNightmareDamage(this));
            stateMachine.RegisterState(new DragonNightmareDie(this));
        }

        private void Start()
        {
            stateMachine.Initialize(DragonNightmareStateID.Idle);

            ResetAttackCollider();
        }

        private void Update()
        {
            if (healthManager.isDead)
            {
                stateMachine.ChangeState(DragonNightmareStateID.Die);
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

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Sword"))
            {
                var playerCore = other.GetComponentInParent<PlayerCore>();

                if (playerCore.stateMachine.StateID == PlayerStateID.AttackSpecial1 || playerCore.stateMachine.StateID == PlayerStateID.AttackSpecial2
                    || playerCore.stateMachine.StateID == PlayerStateID.AttackUltimate)
                {
                    stateMachine.ChangeState(DragonNightmareStateID.Damage);
                }
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

        public void AttackStart(string attackColliderName)
        {
            var attackCollider = attackColliders.FirstOrDefault(attackCollider => attackCollider.name == attackColliderName);

            if (attackCollider == null) { return; }

            attackCollider.SetActive(true);
        }

        public void AttackEnd(string attackColliderName)
        {
            var attackCollider = attackColliders.FirstOrDefault(attackCollider => attackCollider.name == attackColliderName);

            if (attackCollider == null) { return; }

            attackCollider.SetActive(false);
        }

        public void ResetAttackCollider()
        {
            foreach (var attackCollider in attackColliders)
            {
                attackCollider.SetActive(false);
            }
        }
    }
}

