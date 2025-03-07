using Player;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Enemy
{
    public class CoachingSoldierCore : EnemyCoreBase
    {
        [SerializeField] List<GameObject> attackColliders = new List<GameObject>();

        float previousHP;

        const int Fov = 10;     // 視野角
        const int MinSightDistance = 2;

        public TutorialManager tutorialManager;
        public StateMachine<CoachingSoldierStateID> stateMachine;
        public float AngleToPlayer { get; set; }
        public float DistanceToPlayer { get; set; }
        public Vector3 CrossProduct { get; set; }
        public bool IsPlayerInSight => AngleToPlayer <= Fov && DistanceToPlayer >= MinSightDistance;
        public bool CanAttack1 => tutorialManager.currentTask is JustGuardTask && !tutorialManager.currentTask.CheckTask();
        public bool CanAttack2 => tutorialManager.currentTask is JustDodgeTask && !tutorialManager.currentTask.CheckTask();

        void Awake()
        {
            stateMachine = new StateMachine<CoachingSoldierStateID>();
            stateMachine.RegisterState(new CoachingSoldierIdle(this));
            stateMachine.RegisterState(new CoachingSoldierTakeWarning(this));
            stateMachine.RegisterState(new CoachingSoldierMove(this));
            stateMachine.RegisterState(new CoachingSoldierAttack(this));
            stateMachine.RegisterState(new CoachingSoldierDamage(this));
            attackManager.OnEnemykHit += ReceiveDamage;
        }

        void Start()
        {
            stateMachine.Initialize(CoachingSoldierStateID.Idle);

            ResetAttackCollider();
        }

        void Update()
        {
            if (healthManager.IsDead)
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
        }

        void FixedUpdate()
        {
            stateMachine.FixedUpdate();
        }

        void ReceiveDamage()
        {
            stateMachine.ChangeState(CoachingSoldierStateID.Damage);
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

