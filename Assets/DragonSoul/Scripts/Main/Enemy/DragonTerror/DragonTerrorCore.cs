using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Enemy
{
    public class DragonTerrorCore : EnemyCoreBase
    {
        [SerializeField] List<GameObject> attackColliders = new List<GameObject>();
        [SerializeField] Transform breathPoint;
        [SerializeField] GameObject energyBall;
        [SerializeField] Transform eyeTransform;

        Vector3 eyePosition;

        const int Fov = 10;     // 視野角
        const float BreathPower = 20;

        public List<GameObject> energyBalls = new List<GameObject>();
        public StateMachine<DragonTerrorStateID> stateMachine;
        public float AngleToPlayer { get; set; }
        public float DistanceToPlayer { get; set; }
        public Vector3 CrossProduct { get; set; }
        public bool IsPlayerInSight => Mathf.Abs(AngleToPlayer) <= Fov && DistanceToPlayer >= MinDistance;    // プレイヤーが視野内にいるかのフラグ
        public float MinDistance { get; set; }   // 中心から目までの距離
        public bool isFlying = false;
        public bool isJustGuarded = false;
        public Transform attack2EffectTransform;

        void Awake()
        {
            stateMachine = new StateMachine<DragonTerrorStateID>();
            stateMachine.RegisterState(new DragonTerrorIdle(this));
            stateMachine.RegisterState(new DragonTerrorMove(this));
            stateMachine.RegisterState(new DragonTerrorAttack(this));
            stateMachine.RegisterState(new DragonTerrorDamage(this));
            stateMachine.RegisterState(new DragonTerrorDie(this));

            attackManager.OnEnemykHit += ReceiveDamage;
        }

        void Start()
        {
            stateMachine.Initialize(DragonTerrorStateID.Idle);

            InitializeTotalWeight();

            ResetAttackCollider();

            eyePosition = new Vector3(eyeTransform.position.x, transform.position.y, eyeTransform.position.z);
            MinDistance = Vector3.Distance(transform.position, eyePosition);
        }

        void Update()
        {
            if (healthManager.IsDead)
            {
                stateMachine.ChangeState(DragonTerrorStateID.Die);
            }

            stateMachine.Update();

            // プレイヤーとの距離を計算
            Vector3 playerPosition = new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z);
            DistanceToPlayer = Vector3.Distance(transform.position, playerPosition);

            // プレイヤー方向の角度を計算
            Vector3 direction = (playerTransform.position - transform.position).normalized;
            AngleToPlayer = Vector3.SignedAngle(transform.forward, direction, Vector3.up);

            // 外積
            CrossProduct = Vector3.Cross(transform.forward, direction);

            if (playerCore.stateMachine.StateID == PlayerStateID.Block && stateMachine.StateID != DragonTerrorStateID.Damage
                && stateMachine.StateID != DragonTerrorStateID.Die)
            {
                isJustGuarded = true;
            }
            else
            {
                isJustGuarded = false;
            }
        }

        void FixedUpdate()
        {
            stateMachine.FixedUpdate();
        }

        void ReceiveDamage()
        {
            if ((playerCore.stateMachine.StateID == PlayerStateID.AttackSpecial1 || playerCore.stateMachine.StateID == PlayerStateID.AttackSpecial2
                   || playerCore.stateMachine.StateID == PlayerStateID.AttackUltimate) && stateMachine.StateID != DragonTerrorStateID.Die)
            {
                stateMachine.ChangeState(DragonTerrorStateID.Damage);
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

        public void GenerateEnergyBall()
        {
            GameObject breathObj = Instantiate(energyBall, breathPoint.transform.position, Quaternion.identity);
            breathObj.GetComponent<Rigidbody>().velocity = transform.forward * BreathPower;
            energyBalls.Add(breathObj);
            StartCoroutine(DestroyEnergyBall(12, breathObj));
        }

        IEnumerator DestroyEnergyBall(float delay, GameObject breathObj)
        {
            yield return new WaitForSeconds(delay);

            energyBalls.Remove(breathObj);
            Destroy(breathObj);
        }

        public void ClearEnergyBallsList()
        {
            energyBalls.Clear();
        }
    }
}

