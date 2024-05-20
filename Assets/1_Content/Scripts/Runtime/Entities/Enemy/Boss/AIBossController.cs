using BH.Runtime.Managers;
using BH.Runtime.StateMachines;
using BH.Runtime.Systems;
using BH.Scriptables;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace BH.Runtime.Entities
{
    public class AIBossController : Entity, IDamageable
    {
        [Button(ButtonSizes.Large)]
        public void TestMatButton()
        {
            // Ensure the Renderer and its Material are not null
            if (ModelRenderer != null && ModelRenderer.material != null)
            {
                ModelRenderer.material.SetFloat("_GhostColorBoost", 1.0f);
                ModelRenderer.material.SetFloat("_GhostBlend", 1.0f);
            }
            else
            {
                Debug.LogError("Renderer or material not found!");
            }
        }
        
        [field: FoldoutGroup("Stats"), SerializeField, ReadOnly]
        public int CurrentTouchDamage { get; private set; }
        [FoldoutGroup("Stats"), SerializeField]
        private int _initialTouchDamage = 20;
        [field: FoldoutGroup("Stats"), SerializeField]
        public float PushForce { get; private set; } = 10f;
        
        [field: FoldoutGroup("Stats"), SerializeField, HideLabel]
        public Stats Stats { get; private set; }
        
        [field: FoldoutGroup("Projectile Patterns"), SerializeField]
        public ProjectilePatternDataSO PhaseOneCenter { get; private set; }
        [field: FoldoutGroup("Projectile Patterns"), SerializeField]
        public ProjectilePatternDataSO PhasetwoLeft { get; private set; }
        [field: FoldoutGroup("Projectile Patterns"), SerializeField]
        public ProjectilePatternDataSO PhasetwoRight { get; private set; }
        [field: FoldoutGroup("Projectile Patterns"), SerializeField]
        public ProjectilePatternDataSO PhaseThree { get; private set; }

        [field: FoldoutGroup("Animator Params"), SerializeField, HideLabel]
        public AnimatorParams AnimatorParams { get; private set; }
        
        [field: FoldoutGroup("Feedbacks"), SerializeField, HideLabel]
        public EntityFeedbacks Feedbacks { get; private set; }

        [field: BoxGroup("Debug"), SerializeField, ReadOnly]
        public string StateName { get; set; }
        
        [field: Inject(Id = "MainCamera")]
        public Camera Camera { get; }

        [Inject]
        private LevelManager _levelManager;

        // TODO: Add Enemy Signals
        //[Inject]
        //private SignalBus _signalBus;
        
        private EnemySpawner _spawner;

        #region Components
        public SpriteRenderer ModelRenderer { get; private set; }
        public CapsuleCollider2D Collider { get; private set; }
        public Animator Animator { get; private set; }
        public MovementComponent Movement { get; private set; }
        public ShootPatternComponent ShootPattern { get; private set; }
        #endregion

        #region State Machine
        public StateMachine<BossAIState> EnemyHFSM { get; private set; }

        public BossAIMoveOneState MoveOneState { get; private set; }
        public BossAIShootOneState ShootOneState { get; private set; }
        public BossAIMoveTwoState MoveTwoState { get; private set; }
        public BossAIShootTwoState ShootTwoState { get; private set; }
        public BossAIMoveThreeState MoveThreeState { get; private set; }
        public BossAIShootThreeState ShootThreeState { get; private set; }
        
        public BossAIBusyState BusyState { get; private set; }
        public BossAIDeadState DeadState { get; private set; }

        #endregion State Machine

        #region Unity Callbacks

        protected override void Awake()
        {
            base.Awake();
            
            Collider = GetComponent<CapsuleCollider2D>();
            Animator = GetComponentInChildren<Animator>();
            ModelRenderer = Animator.GetComponent<SpriteRenderer>();
            Movement = VerifyComponent<MovementComponent>();
            ShootPattern = VerifyComponent<ShootPatternComponent>();

            EnemyHFSM = new StateMachine<BossAIState>();

            // Active One States
            MoveOneState = new BossAIMoveOneState(this, EnemyHFSM);
            ShootOneState = new BossAIShootOneState(this, EnemyHFSM);
            // Active Two States
            MoveTwoState = new BossAIMoveTwoState(this, EnemyHFSM);
            ShootTwoState = new BossAIShootTwoState(this, EnemyHFSM);
            // Active Three States
            MoveThreeState = new BossAIMoveThreeState(this, EnemyHFSM);
            ShootThreeState = new BossAIShootThreeState(this, EnemyHFSM);
            
            BusyState = new BossAIBusyState(this, EnemyHFSM);
            DeadState = new BossAIDeadState(this, EnemyHFSM);

            EnemyHFSM.Initialize(BusyState);

            // Plan to move to Enemy Spawner
            Stats.ResetStats();
            CurrentTouchDamage = _initialTouchDamage;
        }
        
        private void Start()
        {
            Stats.DiedEvent += OnDied;
        }

        private void Update()
        {
            Stats.LogicUpdate(Time.deltaTime);
            EnemyHFSM.CurrentState.LogicUpdate();
        }
        
        private void FixedUpdate()
        {
            EnemyHFSM.CurrentState.PhysicsUpdate();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            EnemyHFSM.CurrentState.On2DCollisionEnter(other);
        }

        private void OnDestroy()
        {
            Stats.DiedEvent -= OnDied;
        }

        #endregion
        
        public void SetUp(EnemySpawner spawner)
        {
            _spawner = spawner;
            EnemyHFSM.ChangeState(MoveOneState);
        }

        public void HandleDamage(int amount)
        {
            Stats.TakeDamage(amount);
            Feedbacks.HitFeedbackPlayer?.PlayFeedbacks(transform.position, amount);
        }
        
        public void HandleDamageWithForce(int amount, Vector2 direction, float force)
        {
            Stats.TakeDamage(amount);
            //Movement.AddForce(direction, force);
            Feedbacks.HitFeedbackPlayer?.PlayFeedbacks(transform.position, amount);
        }
        
        private void OnDied()
        {
            EnemyHFSM.ChangeState(DeadState);
            Stats.ResetStats();
            _spawner.BossDied(this);
        }

        // TODO: Impliment Health Features
        //private void OnHealthChanged(int maxHealth, int currentHealth)
        //private void OnDied()
    }
}