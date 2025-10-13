using Player;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class DragonUsurperCore : EnemyCoreBase
    {
        [SerializeField] Transform breathPoint;

        public StateMachine<DragonUsurperStateID> stateMachine;
        public AttackSelector<DragonUsurperStateID> attackSelector;
        public Transform attack2EffectTransform;

        protected override void Awake()
        {
            base.Awake();
            stateMachine = new StateMachine<DragonUsurperStateID>();
            stateMachine.RegisterState(new DragonUsurperIdle(this));
            stateMachine.RegisterState(new DragonUsurperMove(this));
            stateMachine.RegisterState(new DragonUsurperAttackBite(this));
            stateMachine.RegisterState(new DragonUsurperAttackClaw(this));
            stateMachine.RegisterState(new DragonUsurperAttackBreath(this));
            stateMachine.RegisterState(new DragonUsurperDamage(this));
            stateMachine.RegisterState(new DragonUsurperDie(this));

            var initialWeights = new Dictionary<DragonUsurperStateID, float>()
            {
                { DragonUsurperStateID.AttackBite, 10f },
                { DragonUsurperStateID.AttackClaw, 10f },
                { DragonUsurperStateID.AttackBreath, 10f }
            };

            attackSelector = new AttackSelector<DragonUsurperStateID>(initialWeights);
        }

        protected override void Start()
        {
            base.Start();

            stateMachine.Initialize(DragonUsurperStateID.Idle);

            ResetAttackCollider();
        }

        protected override void Update()
        {
            base.Update();

            if (healthManager.IsDead)
            {
                stateMachine.ChangeState(DragonUsurperStateID.Die);
            }

            if (isMovable)
            {
                stateMachine.UpdateState();
            }

            if (isFlinch)
            {
                stateMachine.ChangeState(DragonUsurperStateID.Damage);
                isFlinch = false;
            }

            if (isJustGuarded)
            {
                stateMachine.ChangeState(DragonUsurperStateID.Idle);
            }
        }

        void FixedUpdate()
        {
            stateMachine.FixedUpdateState();
        }     
    }
}

