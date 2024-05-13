using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BH.Runtime.Systems
{
    [Serializable]
    public class Stats
    {
        [field: FoldoutGroup("Health"), SerializeField, ReadOnly]
        public int CurrentHealth { get; set; }
        [FoldoutGroup("Health"), SerializeField]
        private int _initialHealth;
        [FoldoutGroup("Health"), SerializeField]
        private bool _isInvincible;
        
        // TODO: Implement shield....
        [field: FoldoutGroup("Shield"), SerializeField, ReadOnly]
        public int CurrentShield { get; set; }
        [FoldoutGroup("Shield"), SerializeField]
        private int _initialShield;
        [FoldoutGroup("Shield"), SerializeField]
        private int _shieldRecoveryRate;

        [field: FoldoutGroup("Speed"), SerializeField, ReadOnly]
        public float CurrentSpeed { get; set; }
        [FoldoutGroup("Speed"), SerializeField]
        private int _initialSpeed;
      
        public Action<int, int> HealthChangedEvent;
        public Action<int, int> ShieldChangedEvent;
        public Action<float, float> SpeedChangedEvent;
        public Action DiedEvent;
        
        public void ResetHealth()
        {
            CurrentHealth = _initialHealth;
            HealthChangedEvent?.Invoke(_initialHealth, CurrentHealth);
        }

        public void TakeDamage(int ammount)
        {
            if (_isInvincible)
                return;
            
            CurrentHealth = (CurrentHealth <= ammount) ? 0 : (CurrentHealth - ammount);
            HealthChangedEvent?.Invoke(_initialHealth, CurrentHealth);

            if (CurrentHealth == 0)
            {
                DiedEvent?.Invoke();
            }
        }
        
        public void SetInvincibility(bool isInvincible)
        {
            _isInvincible = isInvincible;
        }
    }
}