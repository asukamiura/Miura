using Player;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SocialPlatforms.Impl;

namespace Enemy
{
    public class DragonNightmareCore : EnemyCoreBase
    {
        [SerializeField] private List<GameObject> attackColliders = new List<GameObject>();
        [SerializeField] private Transform eyeTransform;

        private Vector3 eyePosition;
        private Vector3 playerPosition;

        private const int Fov = 10;     // 視野角

        public Transform playerTransform;
        public StateMachine<DragonNightmareStateID> stateMachine;
        public float AngleToPlayer { get; private set; }    // プレイヤーのいる角度
        public float DistanceToPlayer { get; private set; } // プレイヤーとの距離
        public Vector3 CrossProduct { get; private set; }
        public bool IsPlayerInSight => Mathf.Abs(AngleToPlayer) <= Fov && DistanceToPlayer >= minDistance;    // プレイヤーが視野内にいるかのフラグ
        public float minDistance;
        public float rotationSpeed = 10;
        public float rotationAngle = 10;

        private void Awake()
        {
            stateMachine = new StateMachine<DragonNightmareStateID>();
            stateMachine.RegisterState(new DragonNightmareIdle(this));
            stateMachine.RegisterState(new DragonNightmareTakeWarning(this));
            stateMachine.RegisterState(new DragonNightmareSearch(this));
            stateMachine.RegisterState(new DragonNightmareLeave(this));
            stateMachine.RegisterState(new DragonNightmareApproach(this));
            stateMachine.RegisterState(new DragonNightmareRetreat(this));
            stateMachine.RegisterState(new DragonNightmareMove(this));            
            stateMachine.RegisterState(new DragonNightmareAttack(this));            
            stateMachine.RegisterState(new DragonNightmareDamage(this));
            stateMachine.RegisterState(new DragonNightmareDie(this));
        }

        private void Start()
        {
            stateMachine.Initialize(DragonNightmareStateID.Idle);

            ResetAttackCollider();

            eyePosition = new Vector3(eyeTransform.position.x, transform.position.y, eyeTransform.position.z);
            minDistance = Vector3.Distance(transform.position, eyePosition);
        }

        private void Update()
        {
            if (healthManager.isDead)
            {
                stateMachine.ChangeState(DragonNightmareStateID.Die);
            }

            stateMachine?.Update();

            // プレイヤーとの距離を計算
            playerPosition = new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z);
            DistanceToPlayer = Vector3.Distance(transform.position, playerPosition);

            // プレイヤー方向の角度を計算
            Vector3 direction = (playerTransform.position - transform.position).normalized;
            AngleToPlayer = Vector3.SignedAngle(transform.forward, direction, Vector3.up);

            // 外積
            //CrossProduct = Vector3.Cross(eyeTransform.forward, direction);
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

        /// <summary>
        /// プレイヤーの方向を向く
        /// </summary>
        public void LookAtPlayer()
        {
            // プレイヤ－の方向を向く
            Vector3 direction = (playerTransform.position - transform.position).normalized;
            Quaternion lookAtRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookAtRotation, rotationSpeed * Time.deltaTime);
        }
    }
}

