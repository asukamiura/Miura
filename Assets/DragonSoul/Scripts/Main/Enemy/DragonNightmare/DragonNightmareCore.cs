using Player;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class DragonNightmareCore : EnemyCoreBase
    {
        public StateMachine<DragonNightmareStateID> stateMachine;
        public Transform attack3EffectTransform;
        public AttackSelector<DragonNightmareStateID> AttackSelector { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            stateMachine = new StateMachine<DragonNightmareStateID>();
            stateMachine.RegisterState(new DragonNightmareIdle(this));
            stateMachine.RegisterState(new DragonNightmareMove(this));
            stateMachine.RegisterState(new DragonNightmareAttackBite(this));
            stateMachine.RegisterState(new DragonNightmareAttackHorn(this));
            stateMachine.RegisterState(new DragonNightmareAttackClaw(this));
            stateMachine.RegisterState(new DragonNightmareDamage(this));
            stateMachine.RegisterState(new DragonNightmareDie(this));

            var initialWeights = new Dictionary<DragonNightmareStateID, float>()
            {
                { DragonNightmareStateID.AttackBite, 10f },
                { DragonNightmareStateID.AttackHorn, 10f },
                { DragonNightmareStateID.AttackClaw, 10f }
            };

            AttackSelector = new AttackSelector<DragonNightmareStateID>(initialWeights);

            attackManager.OnEnemyHit += ReceiveDamage;
        }

        void Start()
        {
            stateMachine.Initialize(DragonNightmareStateID.Idle);

            ResetAttackCollider();
        }

        protected override void Update()
        {
            base.Update();

            if (healthManager.IsDead)
            {
                stateMachine.ChangeState(DragonNightmareStateID.Die);
            }

            if (isMovable)
            {
                stateMachine.StateUpdate();
            }
        }

        void FixedUpdate()
        {
            stateMachine.StateFixedUpdate();
        }

        void ReceiveDamage()
        {
            if ((playerCore.stateMachine.CurrentState == PlayerStateID.AttackSpecial1 || playerCore.stateMachine.CurrentState == PlayerStateID.AttackSpecial2
                   || playerCore.stateMachine.CurrentState == PlayerStateID.AttackUltimate) && stateMachine.CurrentState != DragonNightmareStateID.Die)
            {
                stateMachine.ChangeState(DragonNightmareStateID.Damage);
            }
        }
    }
}

