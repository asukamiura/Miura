using Player;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class DragonTerrorCore : EnemyCoreBase
    {
        [SerializeField] Transform breathPoint;

        public StateMachine<DragonTerrorStateID> stateMachine;
        public AttackSelector<DragonTerrorStateID> AttackSelector { get; private set; }
        public bool isFlying = false;
        public bool isJustGuarded = false;
        public Transform attack2EffectTransform;

        protected override void Awake()
        {
            base.Awake();
            stateMachine = new StateMachine<DragonTerrorStateID>();
            stateMachine.RegisterState(new DragonTerrorIdle(this));
            stateMachine.RegisterState(new DragonTerrorMove(this));
            stateMachine.RegisterState(new DragonTerrorAttackBite(this));
            stateMachine.RegisterState(new DragonTerrorAttackClaw(this));
            stateMachine.RegisterState(new DragonTerrorAttackBreath(this));
            stateMachine.RegisterState(new DragonTerrorDamage(this));
            stateMachine.RegisterState(new DragonTerrorDie(this));

            var initialWeights = new Dictionary<DragonTerrorStateID, float>()
            {
                { DragonTerrorStateID.AttackBite, 10f },
                { DragonTerrorStateID.AttackClaw, 10f },
                { DragonTerrorStateID.AttackBreath, 10f },
            };

            AttackSelector = new AttackSelector<DragonTerrorStateID>(initialWeights);

            attackManager.OnEnemyHit += ReceiveDamage;
        }

        protected override void Start()
        {
            base.Start();

            stateMachine.Initialize(DragonTerrorStateID.Idle);

            ResetAttackCollider();
        }

        protected override void Update()
        {
            base.Update();

            if (healthManager.IsDead)
            {
                stateMachine.ChangeState(DragonTerrorStateID.Die);
            }

            if (isMovable)
            {
                stateMachine.StateUpdate();
            }

            if (playerCore.stateMachine.CurrentState == PlayerStateID.Block && stateMachine.CurrentState != DragonTerrorStateID.Damage
                && stateMachine.CurrentState != DragonTerrorStateID.Die)
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
            stateMachine.StateFixedUpdate();
        }

        void ReceiveDamage()
        {
            if ((playerCore.stateMachine.CurrentState == PlayerStateID.AttackSpecial1 || playerCore.stateMachine.CurrentState == PlayerStateID.AttackSpecial2
                   || playerCore.stateMachine.CurrentState == PlayerStateID.AttackUltimate) && stateMachine.CurrentState != DragonTerrorStateID.Die)
            {
                stateMachine.ChangeState(DragonTerrorStateID.Damage);
            }
        }
    }
}

