using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class DragonUsurperCore : EnemyCoreBase
    {
        [SerializeField] private Transform breathPoint;
        [SerializeField] private GameObject energyBall;

        [SerializeField] public List<GameObject> energyBalls = new List<GameObject>();

        private const int Fov = 10;     // 視野角
        private const int MinSightDistance = 2;

        public Transform playerTransform;
        public StateMachine<DragonUsurperStateID> stateMachine;
        public float AngleToPlayer { get; private set; }
        public float DistanceToPlayer { get; private set; }
        public Vector3 CrossProduct { get; private set; }
        public bool IsPlayerInSight => AngleToPlayer <= Fov && DistanceToPlayer >= MinSightDistance;
        public float rotationSpeed = 1;
        public float rotationAngle = 1;
        public GameObject attack1Collider;
        public GameObject attack2Collider;
        public bool isFlying = false;


        private void Awake()
        {
            stateMachine = new StateMachine<DragonUsurperStateID>();
            stateMachine.RegisterState(new DragonUsurperIdle(this));
            stateMachine.RegisterState(new DragonUsurperTakeWarning(this));
            stateMachine.RegisterState(new DragonUsurperSearch(this));
            stateMachine.RegisterState(new DragonUsurperApproach(this));
            stateMachine.RegisterState(new DragonUsurperTakeOff(this));
            stateMachine.RegisterState(new DragonUsurperLand(this));
            stateMachine.RegisterState(new DragonUsurperLeave(this));
            stateMachine.RegisterState(new DragonUsurperAttack1(this));
            stateMachine.RegisterState(new DragonUsurperAttack2(this));
            stateMachine.RegisterState(new DragonUsurperAttack3(this));
            stateMachine.RegisterState(new DragonUsurperFlyAttack(this));
            stateMachine.RegisterState(new DragonUsurperDie(this));
        }

        private void Start()
        {
            stateMachine.Initialize(DragonUsurperStateID.Idle);

            attack1Collider.SetActive(false);
            attack2Collider.SetActive(false);
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
                    break;
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

