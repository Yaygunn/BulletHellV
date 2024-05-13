using BH.Runtime.Input;
using BH.Runtime.StateMachines;
using BH.Runtime.Systems;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace BH.Runtime.Entities
{
    /// <summary>
    /// This PlayerController class will be more properly set up.. just using for testing...
    /// </summary>
    public class PlayerController : Entity, IDamageable
    {
        [field: FoldoutGroup("Stats"), SerializeField, HideLabel]
        public Stats Stats { get; private set; }

        [field: BoxGroup("Debug"), SerializeField, ReadOnly]
        public string StateName { get; set; }
        
        // TODO: inject this through a construct method...
        [Inject]
        private SignalBus _signalBus;
        
        [field: Inject(Id = "MainCamera")]
        public Camera Camera { get; }
        
        // TODO: this is temporary, pending input system creation...
        //public Vector2 Direction { get; private set; }
        
        public IInputProvider InputProvider { get; private set; }
        
        // TODO: Add animator params..?
        
        // TODO: Do we need virtual cam?
        
        // TODO: Add proper input system.

        #region Components
        
        public MovementComponent Movement { get; private set; }
        public DashComponent Dash { get; private set; }
        public WeaponComponent Weapon { get; private set; }

        #endregion

        #region State Machine
        
        public StateMachine<PlayerState> PlayerHFSM { get; private set; }
        
        public PlayerIdleState IdleState { get; private set; }
        public PlayerMoveState MoveState { get; private set; }
        public PlayerDashState DashState { get; private set; }
        public PlayerBusyState BusyState { get; private set; }
        public PlayerDeadState DeadState { get; private set; }
        
        #endregion State Machine

        #region Unity Callbacks
        
        protected override void Awake()
        {
            base.Awake();
            
            Movement = VerifyComponent<MovementComponent>();
            Dash = VerifyComponent<DashComponent>();
            Weapon = VerifyComponent<WeaponComponent>();
            
            PlayerHFSM = new StateMachine<PlayerState>();
            // Active States
            IdleState = new PlayerIdleState(this, PlayerHFSM);
            MoveState = new PlayerMoveState(this, PlayerHFSM);
            DashState = new PlayerDashState(this, PlayerHFSM);
            
            BusyState = new PlayerBusyState(this, PlayerHFSM);
            DeadState = new PlayerDeadState(this, PlayerHFSM);
            
            PlayerHFSM.Initialize(BusyState);
        }
        
        private void OnEnable()
        {
            Stats.HealthChangedEvent += OnHealthChanged;
            Stats.DiedEvent += OnDied;
        }

        private void Update()
        {
            PlayerHFSM.CurrentState.LogicUpdate();
        }

        private void FixedUpdate()
        {
            PlayerHFSM.CurrentState.PhysicsUpdate();
        }
        
        private void OnDisable()
        {
            Stats.HealthChangedEvent -= OnHealthChanged;
            Stats.DiedEvent -= OnDied;
        }
        
        #endregion

        public void Initialize(IInputProvider inputProvider)
        {
            InputProvider = inputProvider;
        }

        public void Activate()
        {
            Stats.ResetHealth();
            PlayerHFSM.ChangeState(IdleState);
        }

        public void Damage(int amount)
        {
            Stats.TakeDamage(amount);
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