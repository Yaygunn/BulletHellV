using BH.Runtime.Systems;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace BH.Runtime.Test
{
    public class TestHUD : MonoBehaviour
    {
        [SerializeField]
        private Slider _slider;

        [Inject]
        private SignalBus _signalBus;

        private void OnEnable()
        {
            _signalBus.Subscribe<HealthChangedSignal>(OnHealthChanged);
        }

        private void OnDisable()
        {
            _signalBus.Unsubscribe<HealthChangedSignal>(OnHealthChanged);
        }

        private void OnHealthChanged(HealthChangedSignal signal)
        {
            _slider.maxValue = signal.MaxHealth;
            _slider.value = signal.CurrentHealth;
        }
        
        public float GetSliderValue()
        {
            return _slider.value;
        }
    }
}