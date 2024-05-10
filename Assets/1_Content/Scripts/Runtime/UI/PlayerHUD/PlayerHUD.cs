using BH.Runtime.Systems;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace BH.Runtime.UI
{
    public class PlayerHUD : MonoBehaviour
    {
        [BoxGroup("UI Elements"), SerializeField]
        private Slider _slider;

        private SignalBus _signalBus;

        [Inject]
        private void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
            _signalBus.Subscribe<PlayerHealthChangedSignal>(OnHealthChanged);
        }

        private void OnDestroy()
        {
            _signalBus.Unsubscribe<PlayerHealthChangedSignal>(OnHealthChanged);
        }

        private void OnHealthChanged(PlayerHealthChangedSignal signal)
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