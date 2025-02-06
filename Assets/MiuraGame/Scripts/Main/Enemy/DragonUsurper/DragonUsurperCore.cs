using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Enemy
{
    public class DragonUsurperCore : EnemyCoreBase
    {
        [SerializeField] List<GameObject> attackColliders = new List<GameObject>();
        [SerializeField] Transform breathPoint;
        [SerializeField] GameObject energyBall;
        [SerializeField] Transform eyeTransform;

        Vector3 eyePosition;

        const int Fov = 10;     // 視野角

        public List<GameObject> energyBalls = new List<GameObject>();
        public StateMachine<DragonUsurperStateID> stateMachine;
        public float AngleToPlayer { get; set; }
        public float DistanceToPlayer { get; set; }
        public Vector3 CrossProduct { get; set; }
        public bool IsPlayerInSight => Mathf.Abs(AngleToPlayer) <= Fov && DistanceToPlayer >= minDistance;    // プレイヤーが視野内にいるかのフラグ
        public float rotationAngle = 1;
        public float minDistance;   // 中心から目までの距離
        public bool isFlying = false;
        public bool isJustGuarded = false;
        public Transform attack2EffectTransform;

        void Awake()
        {
            stateMachine = new StateMachine<DragonUsurperStateID>();
            stateMachine.RegisterState(new DragonUsurperIdle(this));
            stateMachine.RegisterState(new DragonUsurperTakeWarning(this));
            stateMachine.RegisterState(new DragonUsurperSearch(this));
            stateMachine.RegisterState(new DragonUsurperMove(this));
            stateMachine.RegisterState(new DragonUsurperAttack(this));
            stateMachine.RegisterState(new DragonUsurperDamage(this));
            stateMachine.RegisterState(new DragonUsurperDie(this));

            attackManager.OnEnemykHit += ReceiveDamage;
        }

        void Start()
        {
            stateMachine.Initialize(DragonUsurperStateID.Idle);

            InitializeTotalWeight();

            ResetAttackCollider();

            eyePosition = new Vector3(eyeTransform.position.x, transform.position.y, eyeTransform.position.z);
            minDistance = Vector3.Distance(transform.position, eyePosition);
        }

        void Update()
        {
            if (healthManager.IsDead)
            {
                stateMachine.ChangeState(DragonUsurperStateID.Die);
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

            if (playerCore.stateMachine.StateID == PlayerStateID.Block && stateMachine.StateID != DragonUsurperStateID.Damage
                && stateMachine.StateID != DragonUsurperStateID.Die)
            {
                isJustGuarded = true;
            }
        }

        void FixedUpdate()
        {
            stateMachine.FixedUpdate();
        }

        void ReceiveDamage()
        {
            if ((playerCore.stateMachine.StateID == PlayerStateID.AttackSpecial1 || playerCore.stateMachine.StateID == PlayerStateID.AttackSpecial2
                   || playerCore.stateMachine.StateID == PlayerStateID.AttackUltimate) && stateMachine.StateID != DragonUsurperStateID.Die)
            {
                stateMachine.ChangeState(DragonUsurperStateID.Damage);
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
            Debug.Log("Genarate");
            GameObject breathObj = Instantiate(energyBall, breathPoint.transform.position, Quaternion.identity);
            breathObj.GetComponent<Rigidbody>().velocity = transform.forward * 10;
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

