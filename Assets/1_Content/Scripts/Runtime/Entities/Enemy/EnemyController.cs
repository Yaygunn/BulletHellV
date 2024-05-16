using BH.Runtime.Managers;
using BH.Runtime.StateMachines;
using BH.Runtime.Systems;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem.iOS;
using Zenject;

namespace BH.Runtime.Entities
{
    public class EnemyController : Entity, IDamageable
    {
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

        #region Components
        public MovementComponent Movement { get; private set; }
        #endregion

        #region State Machine
        public StateMachine<EnemyState> EnemyHFSM { get; private set; }

        public EnemyIdleState IdleState { get; private set; }
        public EnemyChaseState ChaseState { get; private set; }

        #endregion State Machine

        #region Unity Callbacks

        protected override void Awake()
        {
            base.Awake();

            Movement = VerifyComponent<MovementComponent>();

            EnemyHFSM = new StateMachine<EnemyState>();

            // Active States
            IdleState = new EnemyIdleState(this, EnemyHFSM);
            ChaseState = new EnemyChaseState(this, EnemyHFSM);

            EnemyHFSM.Initialize(IdleState);

            // Plan to move to Enemy Spawner
            Stats.ResetStats();
        }

        private void Update()
        {
            EnemyHFSM.CurrentState.LogicUpdate();
        }

        private void FixedUpdate()
        {
            EnemyHFSM.CurrentState.PhysicsUpdate();
        }

        #endregion

        public void Damage(int ammount)
        {
            Stats.TakeDamage(ammount);
        }

        // TODO: Impliment Health Features
        //private void OnHealthChanged(int maxHealth, int currentHealth)
        //private void OnDied()

        #region Unity Callbacks
        private void Start()
        {
            AttackTarget = _levelManager.Player.transform;
        }
        #endregion
    }
}