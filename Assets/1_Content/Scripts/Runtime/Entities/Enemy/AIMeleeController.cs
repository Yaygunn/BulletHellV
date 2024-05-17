using System;
using BH.Runtime.Factories;
using BH.Runtime.Managers;
using BH.Runtime.StateMachines;
using BH.Runtime.Systems;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace BH.Runtime.Entities
{
    public class AIMeleeController : Entity, IDamageable
    {
        [field: FoldoutGroup("Stats"), SerializeField]
        public int Damage { get; private set; } = 20;
        [field: FoldoutGroup("Stats"), SerializeField]
        public float PushForce { get; private set; } = 10f;
        
        [field: FoldoutGroup("Stats"), SerializeField, HideLabel]
        public Stats Stats { get; private set; }

        public Transform AttackTarget { get; private set; }


        [field: BoxGroup("Debug"), SerializeField, ReadOnly]
        public string StateName { get; set; }

        [Inject]
        private LevelManager _levelManager;

        // TODO: Add Enemy Signals
        //[Inject]
        //private SignalBus _signalBus;
        
        private AIMeleePool _pool;
        private EnemySpawner _spawner;

        #region Components
        public MovementComponent Movement { get; private set; }
        #endregion

        #region State Machine
        public StateMachine<MeleeAIState> EnemyHFSM { get; private set; }

        public MeleeAIIdleState IdleState { get; private set; }
        public MeleeAIChaseState ChaseState { get; private set; }
        
        public MeleeAIBusyState BusyState { get; private set; }
        public MeleeAIDeadState DeadState { get; private set; }

        #endregion State Machine

        #region Unity Callbacks

        protected override void Awake()
        {
            base.Awake();

            Movement = VerifyComponent<MovementComponent>();

            EnemyHFSM = new StateMachine<MeleeAIState>();

            // Active States
            IdleState = new MeleeAIIdleState(this, EnemyHFSM);
            ChaseState = new MeleeAIChaseState(this, EnemyHFSM);
            
            BusyState = new MeleeAIBusyState(this, EnemyHFSM);
            DeadState = new MeleeAIDeadState(this, EnemyHFSM);

            EnemyHFSM.Initialize(IdleState);

            // Plan to move to Enemy Spawner
            Stats.ResetStats();
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
        
        public void SetPool(AIMeleePool pool)
        {
            _pool = pool;
        }
        
        public void SetUp(EnemySpawner spawner)
        {
            _spawner = spawner;
        }

        public void HandleDamage(int ammount)
        {
            Stats.TakeDamage(ammount);
        }
        
        public void HandleDamageWithForce(int amount, Vector2 direction, float force)
        {
            Stats.TakeDamage(amount);
            Movement.AddForce(direction, force);
        }
        
        private void OnDied()
        {
            Stats.ResetStats();
            EnemyHFSM.ChangeState(BusyState);
            _spawner.EntityDied(this);
            _pool.Despawn(this);
        }

        // TODO: Impliment Health Features
        //private void OnHealthChanged(int maxHealth, int currentHealth)
        //private void OnDied()
    }
}