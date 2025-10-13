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
        }

        protected override void Start()
        {
            base.Start();

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
                stateMachine.UpdateState();
            }

            if (isFlinch)
            {
                stateMachine.ChangeState(DragonNightmareStateID.Damage);
                isFlinch = false;
            }
        }

        void FixedUpdate()
        {
            stateMachine.FixedUpdateState();
        }
    }
}

