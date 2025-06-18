using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class DragonUsurperCore : EnemyCoreBase
    {
        [SerializeField] Transform breathPoint;

        public StateMachine<DragonUsurperStateID> stateMachine;
        public AttackSelector<DragonUsurperStateID> attackSelector;
        public bool isJustGuarded = false;
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

            attackManager.OnEnemyHit += ReceiveDamage;
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
                stateMachine.StateUpdate();
            }

            if (playerCore.stateMachine.CurrentState == PlayerStateID.Block && stateMachine.CurrentState != DragonUsurperStateID.Damage
                && stateMachine.CurrentState != DragonUsurperStateID.Die)
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
                   || playerCore.stateMachine.CurrentState == PlayerStateID.AttackUltimate) && stateMachine.CurrentState != DragonUsurperStateID.Die)
            {
                stateMachine.ChangeState(DragonUsurperStateID.Damage);
            }
        }  
    }
}

