using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BH.Runtime.Systems
{
    [Serializable]
    public class Health
    {
        [SerializeField]
        private int _initialHealth;

        [SerializeField, ReadOnly]
        private int _currentHealth;
        
        public Action<int, int> HealthChangedEvent;
        public Action DiedEvent;
        
        public void ResetHealth()
        {
            _currentHealth = _initialHealth;
            HealthChangedEvent?.Invoke(_initialHealth, _currentHealth);
        }

        public void TakeDamage(int ammount)
        {
            _currentHealth = (_currentHealth <= ammount) ? 0 : (_currentHealth - ammount);
            HealthChangedEvent?.Invoke(_initialHealth, _currentHealth);

            if (_currentHealth == 0)
            {
                DiedEvent?.Invoke();
            }
        }
    }
}