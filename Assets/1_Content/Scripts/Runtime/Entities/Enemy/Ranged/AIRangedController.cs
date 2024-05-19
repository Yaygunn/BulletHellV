using System;
using System.Collections.Generic;
using BH.Runtime.Entities.States.Active;
using BH.Runtime.Factories;
using BH.Runtime.Managers;
using BH.Runtime.StateMachines;
using BH.Runtime.Systems;
using BH.Scriptables;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;
using MoreMountains.Tools;

namespace BH.Runtime.Entities
{
    public class AIRangedController : Entity, IDamageable
    {
        [field: FoldoutGroup("Stats"), SerializeField, ReadOnly]
        public int CurrentTouchDamage { get; private set; }
        [FoldoutGroup("Stats"), SerializeField]
        private int _initialTouchDamage = 20;
        [field: FoldoutGroup("Stats"), SerializeField]
        public float PushForce { get; private set; } = 10f;

        [field: FoldoutGroup("Stats"), SerializeField, HideLabel]
        public Stats Stats { get; private set; }

        [field: FoldoutGroup("Projectile Patterns"), SerializeField]
        public List<ProjectilePatternDataSO> ProjectilePatterns { get; private set; }

        public Transform AttackTarget { get; private set; }

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

        private AIRangedPool _pool;
        private bool _inPool;
        private EnemySpawner _spawner;

        #region Components
        public SpriteRenderer ModelRenderer { get; private set; }
        public CapsuleCollider2D Collider { get; private set; }
        public Animator Animator { get; private set; }
        public MovementComponent Movement { get; private set; }
        public ShootPatternComponent ShootPattern { get; private set; }
        #endregion

        #region State Machine
        public StateMachine<RangedAIState> EnemyHFSM { get; private set; }

        public RangedAIMoveState MoveState { get; private set; }
        public RangedAIShootState ShootState { get; private set; }

        public RangedAIBusyState BusyState { get; private set; }
        public RangedAIDeadState DeadState { get; private set; }

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

            EnemyHFSM = new StateMachine<RangedAIState>();

            // Active States
            MoveState = new RangedAIMoveState(this, EnemyHFSM);
            ShootState = new RangedAIShootState(this, EnemyHFSM);

            BusyState = new RangedAIBusyState(this, EnemyHFSM);
            DeadState = new RangedAIDeadState(this, EnemyHFSM);

            EnemyHFSM.Initialize(MoveState);

            // Plan to move to Enemy Spawner
            Stats.ResetStats();
            CurrentTouchDamage = _initialTouchDamage;
        }

        private void Start()
        {
            AttackTarget = _levelManager.Player.transform;
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

        public void SetPool(AIRangedPool pool)
        {
            _pool = pool;
        }

        public void SetUp(EnemySpawner spawner)
        {
            _spawner = spawner;
            _inPool = false;
            Wave wave = _spawner.GetCurrentWave();
            Stats.ModifyStatsManual(wave.HealthMultiplier, wave.ShieldMultiplier, wave.SpeedMultiplier);
            CurrentTouchDamage = (int)(CurrentTouchDamage * wave.DamageMultiplier);
            Stats.Initialize();
        }

        public void HandleDamage(int amount)
        {
            Stats.TakeDamage(amount);
            Feedbacks.HitFeedbackPlayer?.PlayFeedbacks(transform.position, amount);
        }

        public void HandleDamageWithForce(int amount, Vector2 direction, float force)
        {
            Stats.TakeDamage(amount);
            Movement.AddForce(direction, force);
            Feedbacks.HitFeedbackPlayer?.PlayFeedbacks(transform.position, amount);
        }

        private void OnDied()
        {
            EnemyHFSM.ChangeState(DeadState);
        }

        public void ReturnToPool()
        {
            Stats.ResetStats();
            CurrentTouchDamage = _initialTouchDamage;
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