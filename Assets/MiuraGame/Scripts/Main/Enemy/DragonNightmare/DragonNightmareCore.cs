using Player;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Enemy
{
    public class DragonNightmareCore : EnemyCoreBase
    {
        [SerializeField] List<GameObject> attackColliders = new List<GameObject>();
        [SerializeField] Transform eyeTransform;

        Vector3 eyePosition;

        const int Fov = 10;     // 視野角

        public StateMachine<DragonNightmareStateID> stateMachine;
        public float AngleToPlayer { get; set; }    // プレイヤーのいる角度
        public float DistanceToPlayer { get; set; } // プレイヤーとの距離
        public Vector3 CrossProduct { get; set; }
        public bool IsPlayerInSight => Mathf.Abs(AngleToPlayer) <= Fov && DistanceToPlayer >= minDistance;    // プレイヤーが視野内にいるかのフラグ
        public float minDistance;
        public float rotationAngle = 10;
        public bool isJustGuarded = false;
        public Transform attack3EffectTransform;

        void Awake()
        {
            stateMachine = new StateMachine<DragonNightmareStateID>();
            stateMachine.RegisterState(new DragonNightmareIdle(this));
            stateMachine.RegisterState(new DragonNightmareTakeWarning(this));
            stateMachine.RegisterState(new DragonNightmareSearch(this));
            stateMachine.RegisterState(new DragonNightmareMove(this));
            stateMachine.RegisterState(new DragonNightmareAttack(this));
            stateMachine.RegisterState(new DragonNightmareDamage(this));
            stateMachine.RegisterState(new DragonNightmareDie(this));
            attackManager.OnEnemykHit += ReceiveDamage;
        }

        void Start()
        {
            stateMachine.Initialize(DragonNightmareStateID.Idle);

            InitializeTotalWeight();

            ResetAttackCollider();

            eyePosition = new Vector3(eyeTransform.position.x, transform.position.y, eyeTransform.position.z);
            minDistance = Vector3.Distance(transform.position, eyePosition);
        }

        void Update()
        {
            if (healthManager.IsDead)
            {
                stateMachine.ChangeState(DragonNightmareStateID.Die);
            }

            stateMachine?.Update();

            // プレイヤーとの距離を計算
            Vector3 playerPosition = new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z);
            DistanceToPlayer = Vector3.Distance(transform.position, playerPosition);

            // プレイヤー方向の角度を計算
            Vector3 direction = (playerTransform.position - transform.position).normalized;
            AngleToPlayer = Vector3.SignedAngle(transform.forward, direction, Vector3.up);

            if (playerCore.stateMachine.StateID == PlayerStateID.Block && stateMachine.StateID != DragonNightmareStateID.Damage
                && stateMachine.StateID != DragonNightmareStateID.Die)
            {
                isJustGuarded = true;
            }
        }

        void FixedUpdate()
        {
            stateMachine.FixedUpdate();
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Sword"))
            {
                if ((playerCore.stateMachine.StateID == PlayerStateID.AttackSpecial1 || playerCore.stateMachine.StateID == PlayerStateID.AttackSpecial2
                    || playerCore.stateMachine.StateID == PlayerStateID.AttackUltimate) && stateMachine.StateID != DragonNightmareStateID.Die)
                {
                    stateMachine.ChangeState(DragonNightmareStateID.Damage);
                }
            }
        }

        void ReceiveDamage()
        {
            if ((playerCore.stateMachine.StateID == PlayerStateID.AttackSpecial1 || playerCore.stateMachine.StateID == PlayerStateID.AttackSpecial2
                   || playerCore.stateMachine.StateID == PlayerStateID.AttackUltimate) && stateMachine.StateID != DragonNightmareStateID.Die)
            {
                stateMachine.ChangeState(DragonNightmareStateID.Damage);
            }
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

