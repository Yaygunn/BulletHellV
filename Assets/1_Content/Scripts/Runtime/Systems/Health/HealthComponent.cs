using BH.Runtime.Systems;
using MEC;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace BH.Runtime.Systems
{
    public class HealthComponent : MonoBehaviour, IDamageable
    {
        [SerializeField]
        private int _initialHealth;

        [SerializeField, ReadOnly]
        private int _currentHealth;

        [Inject]
        private SignalBus _signalBus;

        private void Start()
        {
            ResetHealth();
        }

        [Button("Reset Health")]
        public void ResetHealth()
        {
            _currentHealth = _initialHealth;
            _signalBus.Fire(new HealthChangedSignal(_initialHealth, _currentHealth));
        }

        public void Damage(int ammount)
        {
            // subtract ammount from health, or set to 0
            _currentHealth = (_currentHealth <= ammount) ? 0 : (_currentHealth - ammount);
            _signalBus.Fire(new HealthChangedSignal(_initialHealth, _currentHealth));

            if(_currentHealth == 0)
            {
                Timing.RunCoroutine(RespawnPlayerCoroutine());
            }
        }

        private IEnumerator<float> RespawnPlayerCoroutine()
        {
            gameObject.SetActive(false);
            yield return Timing.WaitForSeconds(5);
            gameObject.SetActive(true);
            gameObject.transform.position = Vector3.zero;
            ResetHealth();
        }
    }
}