using System;
using BH.Runtime.Factories;
using BH.Runtime.Managers;
using BH.Runtime.StateMachines;
using BH.Runtime.Systems;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;
using MoreMountains.Tools;

namespace BH.Runtime.Entities
{
    public class AIMeleeController : Entity, IDamageable
    {
        [field: FoldoutGroup("Stats"), SerializeField, ReadOnly]
        public int CurrentDamage { get; private set; }
        [FoldoutGroup("Stats"), SerializeField]
        private int _initialDamage = 20;

        [field: FoldoutGroup("Stats"), SerializeField]
        public float AttackRange { get; private set; } = 1f;

        [field: FoldoutGroup("Stats"), SerializeField]
        public float PushForce { get; private set; } = 10f;

        [field: FoldoutGroup("Stats"), SerializeField, HideLabel]
        public Stats Stats { get; private set; }

        //Health
        public MMHealthBar healthBar;

        public PlayerController PlayerTarget { get; private set; }

        [field: FoldoutGroup("Animator Params"), SerializeField, HideLabel]
        public AnimatorParams AnimatorParams { get; private set; }

        [field: FoldoutGroup("Feedbacks"), SerializeField, HideLabel]
        public EntityFeedbacks Feedbacks { get; private set; }



        [field: BoxGroup("Debug"), SerializeField, ReadOnly]
        public string StateName { get; set; }

        [Inject]
        private LevelManager _levelManager;

        // TODO: Add Enemy Signals
        //[Inject]
        //private SignalBus _signalBus;

        private AIMeleePool _pool;
        private bool _inPool;
        private EnemySpawner _spawner;

        #region Components
        public SpriteRenderer ModelRenderer { get; private set; }
        public CapsuleCollider2D Collider { get; private set; }
        public Animator Animator { get; private set; }
        public MovementComponent Movement { get; private set; }
        #endregion

        #region State Machine
        public StateMachine<MeleeAIState> EnemyHFSM { get; private set; }

        public MeleeAIAttackState AttackState { get; private set; }
        public MeleeAIChaseState ChaseState { get; private set; }

        public MeleeAIBusyState BusyState { get; private set; }
        public MeleeAIDeadState DeadState { get; private set; }

        #endregion State Machine

        #region Unity Callbacks

        protected override void Awake()
        {
            base.Awake();

            Collider = GetComponent<CapsuleCollider2D>();
            Animator = GetComponentInChildren<Animator>();
            ModelRenderer = Animator.GetComponent<SpriteRenderer>();
            Movement = VerifyComponent<MovementComponent>();

            EnemyHFSM = new StateMachine<MeleeAIState>();

            // Active States
            AttackState = new MeleeAIAttackState(this, EnemyHFSM);
            ChaseState = new MeleeAIChaseState(this, EnemyHFSM);

            BusyState = new MeleeAIBusyState(this, EnemyHFSM);
            DeadState = new MeleeAIDeadState(this, EnemyHFSM);

            EnemyHFSM.Initialize(BusyState);

            // Plan to move to Enemy Spawner
            Stats.ResetStats();
            CurrentDamage = _initialDamage;
        }

        private void Start()
        {
            PlayerTarget = _levelManager.Player;
            Stats.DiedEvent += OnDied;
        }

        private void Update()
        {
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

        public void SetPool(AIMeleePool pool)
        {
            _pool = pool;
        }

        public void SetUp(EnemySpawner spawner)
        {
            _spawner = spawner;
            _inPool = false;
            ModelRenderer.color = Color.white;
            Wave wave = _spawner.GetCurrentWave();
            Stats.ModifyStatsManual(wave.HealthMultiplier, wave.ShieldMultiplier, wave.SpeedMultiplier);
            CurrentDamage = (int)(CurrentDamage * wave.DamageMultiplier);
        }

        public void HandleDamage(int ammount)
        {
            Stats.TakeDamage(ammount);
            healthBar?.UpdateBar(Stats.CurrentHealth, 0, Stats.MaxHealth, true);
            Feedbacks.HitFeedbackPlayer?.PlayFeedbacks(this.transform.position, ammount);
        }

        public void HandleDamageWithForce(int amount, Vector2 direction, float force)
        {
            Stats.TakeDamage(amount);
            Movement.AddForce(direction, force);
        }

        private void OnDied()
        {
            EnemyHFSM.ChangeState(DeadState);
        }

        public void ReturnToPool()
        {
            Stats.ResetStats();
            CurrentDamage = _initialDamage;
            _spawner.EntityDied(this);

            if (_inPool) return;

            _inPool = true;
            _pool.Despawn(this);
        }

        // TODO: Impliment Health Features
        //private void OnHealthChanged(int maxHealth, int currentHealth)
        //private void OnDied()
    }
}