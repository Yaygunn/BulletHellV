using System;
using BH.Runtime.StateMachines;
using BH.Runtime.Systems;
using BH.Runtime.Test;
using UnityEngine;
using Zenject;

namespace BH.Runtime.Entities
{
    /// <summary>
    /// This PlayerController class will be more properly set up.. just using for testing...
    /// </summary>
    public class PlayerController : Entity, IDamageable
    {
        [field: SerializeField]
        public Health Health { get; private set; }
        
        // TODO: inject this through a construct method...
        [Inject]
        private SignalBus _signalBus;
        
        // TODO: this is temporary, pending input system creation...
        public Vector2 Direction { get; private set; }
        
        // TODO: Add animator params..?
        
        // TODO: Do we need virtual cam?
        
        // TODO: Add proper input system.

        #region Components
        
        public MovementComponent Movement { get; private set; }

        #endregion

        #region State Machine
        
        public StateMachine<PlayerState> PlayerHFSM { get; private set; }
        
        public PlayerActiveState ActiveState { get; private set; }
        public PlayerBusyState BusyState { get; private set; }
        public PlayerDeadState DeadState { get; private set; }
        
        #endregion State Machine

        #region Unity Callbacks
        
        protected override void Awake()
        {
            base.Awake();
            
            Movement = VerifyComponent<MovementComponent>();
            
            PlayerHFSM = new StateMachine<PlayerState>();
            ActiveState = new PlayerActiveState(this, PlayerHFSM);
            BusyState = new PlayerBusyState(this, PlayerHFSM);
            DeadState = new PlayerDeadState(this, PlayerHFSM);
            
            PlayerHFSM.Initialize(BusyState);
        }
        
        private void OnEnable()
        {
            Health.HealthChangedEvent += OnHealthChanged;
            Health.DiedEvent += OnDied;
        }

        private void Update()
        {
            // TODO: This will be redone with the new input system.
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            
            Direction = new Vector2(horizontal, vertical).normalized;
            
            PlayerHFSM.CurrentState.LogicUpdate();
        }

        private void FixedUpdate()
        {
            PlayerHFSM.CurrentState.PhysicsUpdate();
        }
        
        private void OnDisable()
        {
            Health.HealthChangedEvent -= OnHealthChanged;
            Health.DiedEvent -= OnDied;
        }
        
        #endregion

        public void Activate()
        {
            Health.ResetHealth();
            PlayerHFSM.ChangeState(ActiveState);
        }

        public void Damage(int amount)
        {
            Health.TakeDamage(amount);
        }
        
        private void OnHealthChanged(int maxHealth, int currentHealth)
        {
            _signalBus.Fire(new PlayerHealthChangedSignal(maxHealth, currentHealth));
        }
        
        private void OnDied()
        {
            // TODO: need to request state change here too maybe?
            PlayerHFSM.ChangeState(DeadState);
            _signalBus.Fire(new PlayerDiedSignal());
        }
    }
}