using System.Collections.Generic;
using BH.Runtime.Audio;
using BH.Runtime.Input;
using BH.Runtime.StateMachines;
using BH.Runtime.Systems;
using MEC;
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
        
        [field: FoldoutGroup("Animator Params"), SerializeField, HideLabel]
        public AnimatorParams AnimatorParams { get; private set; }
        
        [field: FoldoutGroup("Feedbacks"), SerializeField, HideLabel]
        public EntityFeedbacks Feedbacks { get; private set; }

        [field: BoxGroup("Debug"), SerializeField, ReadOnly]
        public string StateName { get; set; }
        
        // TODO: inject this through a construct method...
        [Inject]
        private SignalBus _signalBus;
        
        [field: Inject(Id = "MainCamera")]
        public Camera Camera { get; }
        
        // TODO: this is temporary, pending input system creation...
        //public Vector2 Direction { get; private set; }
        
        public bool IsFacingRight => transform.localScale.x > 0;
        
        public IInputProvider InputProvider { get; private set; }
        public IWwiseEventHandler WwiseEventHandler { get; private set; }
        
        // TODO: Add animator params..?
        
        // TODO: Do we need virtual cam?
        
        // TODO: Add proper input system.

        #region Components
        
        public SpriteRenderer ModelRenderer { get; private set; } 
        public Rigidbody2D Rigidbody { get; private set; }
        public CapsuleCollider2D Collider { get; private set; }
        public Animator Animator { get; private set; }
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
            
            Rigidbody = GetComponent<Rigidbody2D>();
            Collider = GetComponent<CapsuleCollider2D>();
            Animator = GetComponentInChildren<Animator>();
            ModelRenderer = Animator.GetComponent<SpriteRenderer>();
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
            Stats.ShieldChangedEvent += OnShieldChanged;
            Stats.DiedEvent += OnDied;
        }

        private void Update()
        {
            Stats.LogicUpdate(Time.deltaTime);
            PlayerHFSM.CurrentState.LogicUpdate();
        }

        private void FixedUpdate()
        {
            PlayerHFSM.CurrentState.PhysicsUpdate();
        }
        
        private void OnDisable()
        {
            Stats.HealthChangedEvent -= OnHealthChanged;
            Stats.ShieldChangedEvent -= OnShieldChanged;
            Stats.DiedEvent -= OnDied;
        }
        
        #endregion

        public void Initialize(IInputProvider inputProvider, IWwiseEventHandler wwiseEventHandler)
        {
            InputProvider = inputProvider;
            WwiseEventHandler = wwiseEventHandler;
        }

        public void Activate(bool isRespawn)
        {
            Stats.ResetStats();
            PlayerHFSM.ChangeState(IdleState);
            Timing.RunCoroutine(InvulnerableTimerCoroutine(1f).CancelWith(gameObject));
            InputProvider.EnablePlayerControls();
            
            if (isRespawn)
            {
                Feedbacks.RespawnFeedbackPlayer?.PlayFeedbacks();
            }
        }
        
        private IEnumerator<float> InvulnerableTimerCoroutine(float duration)
        {
            Stats.SetInvincibility(true);
            yield return Timing.WaitForSeconds(duration);
            Feedbacks.RespawnFeedbackPlayer?.StopFeedbacks();
            Stats.SetInvincibility(false);
        }

        public void HandleDamage(int amount)
        {
            if (Stats.TakeDamage(amount))
            {
                WwiseEventHandler.PostAudioEvent(PlayerSFX.Hurt, gameObject);
                Feedbacks.HitFeedbackPlayer?.PlayFeedbacks();
            }
        }
        
        public void HandleDamageWithForce(int amount, Vector2 direction, float force)
        {
            if (Stats.TakeDamage(amount))
            {
                WwiseEventHandler.PostAudioEvent(PlayerSFX.Hurt, gameObject);
                Feedbacks.HitFeedbackPlayer?.PlayFeedbacks();
                //Movement.AddForce(direction, force);
            }
        }

        public void FlipCharacter(bool faceRight)
        {
            transform.localScale = faceRight ? new Vector3(
                Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z) 
                : new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }

        private void OnHealthChanged(int maxHealth, int currentHealth)
        {
            _signalBus.Fire(new PlayerHealthChangedSignal(maxHealth, currentHealth));
        }
        
        private void OnShieldChanged(int maxShield, int currentShield)
        {
            _signalBus.Fire(new PlayerShieldChangedSignal(maxShield, currentShield));
        }
        
        private void OnDied()
        {
            PlayerHFSM.ChangeState(DeadState);
        }
        
        public void HandlePlayerDeath()
        {
            _signalBus.Fire(new PlayerDiedSignal());
        }
    }
}