namespace Enemy
{
    public class CoachingSoldierCore : EnemyCoreBase
    {
        float previousHP;

        public StateMachine<CoachingSoldierStateID> stateMachine;
        public TutorialTaskManager tutorialTaskManager;
        public bool CanAttack1 => tutorialTaskManager.currentTask is JustGuardTask && !tutorialTaskManager.currentTask.CheckTask();
        public bool CanAttack2 => tutorialTaskManager.currentTask is JustDodgeTask && !tutorialTaskManager.currentTask.CheckTask();

        protected override void Awake()
        {
            base.Awake();
            stateMachine = new StateMachine<CoachingSoldierStateID>();
            stateMachine.RegisterState(new CoachingSoldierIdle(this));
            stateMachine.RegisterState(new CoachingSoldierTakeWarning(this));
            stateMachine.RegisterState(new CoachingSoldierMove(this));
            stateMachine.RegisterState(new CoachingSoldierAttack(this));
            stateMachine.RegisterState(new CoachingSoldierDamage(this));
        }

        protected override void Start()
        {
            base.Start();

            stateMachine.Initialize(CoachingSoldierStateID.Idle);

            ResetAttackCollider();
        }

        protected override void Update()
        {
            base.Update();

            if (isMovable)
            {
                stateMachine.UpdateState();
            }

            if (isFlinch)
            {
                stateMachine.ChangeState(CoachingSoldierStateID.Damage);
                isFlinch = false;
            }

            if (healthManager.CurrentHP != previousHP)
            {
                healthManager.RestoreHP(9999);
                previousHP = healthManager.CurrentHP;
            }
        }

        void FixedUpdate()
        {
            stateMachine.FixedUpdateState();
        }
    }
}

